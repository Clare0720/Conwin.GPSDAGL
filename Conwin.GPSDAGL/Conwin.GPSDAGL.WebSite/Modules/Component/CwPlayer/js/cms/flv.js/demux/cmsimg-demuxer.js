function Swap16(src) {
    return (((src >>> 8) & 0xFF) |
            ((src & 0xFF) << 8));
}

function Swap32(src) {
    return (((src & 0xFF000000) >>> 24) |
            ((src & 0x00FF0000) >>> 8)  |
            ((src & 0x0000FF00) << 8)   |
            ((src & 0x000000FF) << 24));
}

function ReadBig32(array, index) {
    return ((array[index] << 24)     |
            (array[index + 1] << 16) |
            (array[index + 2] << 8)  |
            (array[index + 3]));
}


function CMSIMGDemuxer(probeData, config) {
    this.cms = new CMS();

    this.mp3enc = new lamejs.Mp3Encoder(1, 8000, 128);
    
    this.TAG = 'CMSIMGDemuxer';

    this._config = config;
    this._scriptPassed = false;
    this._onError = null;
    this._onMediaInfo = null;
    this._onMetaDataArrived = null;
    this._onScriptDataArrived = null;
    this._onTrackMetadata = null;
    this._onDataAvailable = null;

    this._dataOffset = probeData.dataOffset;
    this._firstParse = true;
    this._dispatch = false;

    this._hasAudio = probeData.hasAudioTrack;
    this._hasVideo = probeData.hasVideoTrack;

    this._hasAudioFlagOverrided = false;
    this._hasVideoFlagOverrided = false;

    this._audioInitialMetadataDispatched = false;
    this._videoInitialMetadataDispatched = false;

    this._mediaInfo = new MediaInfo();
    this._mediaInfo.hasAudio = this._hasAudio;
    this._mediaInfo.hasVideo = this._hasVideo;
    this._metadata = null;
    this._audioMetadata = null;
    this._videoMetadata = null;

    this._naluLengthSize = 4;
    this._timestampBase = 0;  // int32, in milliseconds
    this._timescale = 1000;
    this._duration = 0;  // int32, in milliseconds
    this._durationOverrided = false;
    this._referenceFrameRate = {
        fixed: true,
        fps: 23.976,
        fps_num: 23976,
        fps_den: 1000
    };

    this._cmsSoundRateTable = [5500, 11025, 22050, 44100, 48000];

    this._mpegSamplingRates = [
        96000, 88200, 64000, 48000, 44100, 32000,
        24000, 22050, 16000, 12000, 11025, 8000, 7350
    ];

    this._mpegAudioV10SampleRateTable = [44100, 48000, 32000, 0];
    this._mpegAudioV20SampleRateTable = [22050, 24000, 16000, 0];
    this._mpegAudioV25SampleRateTable = [11025, 12000, 8000,  0];

    this._mpegAudioL1BitRateTable = [0, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, -1];
    this._mpegAudioL2BitRateTable = [0, 32, 48, 56,  64,  80,  96, 112, 128, 160, 192, 224, 256, 320, 384, -1];
    this._mpegAudioL3BitRateTable = [0, 32, 40, 48,  56,  64,  80,  96, 112, 128, 160, 192, 224, 256, 320, -1];

    this._videoTrack = {type: 'jpeg', id: 1, sequenceNumber: 0, samples: [], length: 0};
    this._audioTrack = {type: 'audio', id: 2, sequenceNumber: 0, samples: [], length: 0};

    this._littleEndian = (function () {
        let buf = new ArrayBuffer(2);
        (new DataView(buf)).setInt16(0, 256, true);  // little-endian write
        return (new Int16Array(buf))[0] === 256;  // platform-spec read, if equal then LE
    })();
}
CMSIMGDemuxer.probe = function (buffer) {
    let data = new Uint8Array(buffer);
    var s = String.fromCharCode.apply(null, data);
    var offset = s.indexOf("\r\n\r\n");
    let mismatch = {match: false};

    if (offset < 0) {
        return mismatch;
    }
    var hasAudio = false;
    var hasVideo = false;
    
    var lines = s.substr(0, offset).split("\r\n");
    var headers = {};
    for (var i = 0; i < lines.length; i++) {
        var f = lines[i].split(':');
        var key = f.shift();
        var value = f.join(':');
        headers[key] = value;
    }
    if (headers.track) {
        var tracks = headers.track.split(';');
        headers.tracks = [];
        for (var i = 0; i < tracks.length; i++) {
            var fields = tracks[i].split(',');
            var track = {};
            for (var j = 0; j < fields.length; j++) {
                var f = fields[j].split('=');
                var key = f.shift();
                var value = f.join('=');
                track[key] = value;
                if (key === 'codec') {
                    if (value === 'alaw') {
                        hasAudio = true;
                    } else if (value === 'jpg') {
                        hasVideo = true;
                    }
                }
            }
            headers.tracks.push(track);
        }
    }
    if (!hasAudio && !hasVideo) {
        return mismatch;
    }

    return {
        match: true,
        consumed: offset,
        dataOffset: offset,
        hasAudioTrack: hasAudio,
        hasVideoTrack: hasVideo
    };
}



CMSIMGDemuxer.prototype.destroy = function () {
    this._mediaInfo = null;
    this._metadata = null;
    this._audioMetadata = null;
    this._videoMetadata = null;
    this._videoTrack = null;
    this._audioTrack = null;

    this._onError = null;
    this._onMediaInfo = null;
    this._onMetaDataArrived = null;
    this._onScriptDataArrived = null;
    this._onTrackMetadata = null;
    this._onDataAvailable = null;
}

CMSIMGDemuxer.prototype.bindDataSource = function (loader) {
    loader.onDataArrival = this.parseChunks.bind(this);
    return this;
}

// prototype: function(type: string, metadata: any): void
CMSIMGDemuxer.prototype.onTrackMetadata = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onTrackMetadata;
    } else {
        this._onTrackMetadata = callback;
    }
}

// prototype: function(mediaInfo: MediaInfo): void
CMSIMGDemuxer.prototype.onMediaInfo = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onMediaInfo;
    } else {
        this._onMediaInfo = callback;
    }
}

CMSIMGDemuxer.prototype.onMetaDataArrived = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onMetaDataArrived;
    } else {
        this._onMetaDataArrived = callback;
    }
}

CMSIMGDemuxer.prototype.onScriptDataArrived = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onScriptDataArrived;
    } else {
        this._onScriptDataArrived = callback;
    }
}

// prototype: function(type: number, info: string): void
CMSIMGDemuxer.prototype.onError = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onError;
    } else {
        this._onError = callback;
    }
}

// prototype: function(videoTrack: any, audioTrack: any): void
CMSIMGDemuxer.prototype.onDataAvailable = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onDataAvailable;
    } else {
        this._onDataAvailable = callback;
    }
}

// timestamp base for output samples, must be in milliseconds
CMSIMGDemuxer.prototype.timestampBase = function (base) {
    if (typeof base === 'undefined') {
        return this._timestampBase;
    } else {
        this._timestampBase = base;
    }
}

CMSIMGDemuxer.prototype.overridedDuration = function (duration) {
    if (typeof duration === 'undefined') {
        return this._duration;
    } else {
        // Force-override media duration. Must be in milliseconds, int32
        this._durationOverrided = true;
        this._duration = duration;
        this._mediaInfo.duration = duration;
    }
}

// Force-override audio track present flag, boolean
CMSIMGDemuxer.prototype.overridedHasAudio = function (hasAudio) {
    this._hasAudioFlagOverrided = true;
    this._hasAudio = hasAudio;
    this._mediaInfo.hasAudio = hasAudio;
}

// Force-override video track present flag, boolean
CMSIMGDemuxer.prototype.overridedHasVideo = function (hasVideo) {
    this._hasVideoFlagOverrided = true;
    this._hasVideo = hasVideo;
    this._mediaInfo.hasVideo = hasVideo;
}

CMSIMGDemuxer.prototype.resetMediaInfo = function () {
    this._mediaInfo = new MediaInfo();
}

CMSIMGDemuxer.prototype._isInitialMetadataDispatched = function () {
    if (this._hasAudio && this._hasVideo) {  // both audio & video
        return this._audioInitialMetadataDispatched && this._videoInitialMetadataDispatched;
    }
    if (this._hasAudio && !this._hasVideo) {  // audio only
        return this._audioInitialMetadataDispatched;
    }
    if (!this._hasAudio && this._hasVideo) {  // video only
        return this._videoInitialMetadataDispatched;
    }
    return false;
}

// function parseChunks(chunk: ArrayBuffer, byteStart: number): number;
CMSIMGDemuxer.prototype.parseChunks = function (chunk, byteStart) {
    // if (!this._onError || !this._onMediaInfo || !this._onTrackMetadata || !this._onDataAvailable) {
    //     throw new IllegalStateException('Cms: onError & onMediaInfo & onTrackMetadata & onDataAvailable callback must be specified');
    // }
    let data = new Uint8Array(chunk);
    this.cms.parse(data);
    while ((part = this.cms.parts.shift())) {
        if (!this._scriptPassed) {
            this._parseScriptData();
            this._scriptPassed = true;
        }
        this._dispatch = true;
        switch (part.header.type) {
        case this.cms.CMS_PART_JPG:     // JPEG
            this._parseIMGData(part);
            break;
        }

    }

    // dispatch parsed frames to consumer (typically, the remuxer)
    if (this._isInitialMetadataDispatched()) {
        if (this._dispatch && (this._audioTrack.length || this._videoTrack.length)) {
            this._onDataAvailable(this._audioTrack, this._videoTrack);
        }
    }

    return chunk.byteLength;
}

CMSIMGDemuxer.prototype._parseScriptData = function () {
    this._metadata = {};
    this._hasAudio                    = this.cms.header.hasAudio;
    this._mediaInfo.hasAudio          = this._hasAudio;
    this._hasVideo                    = true;
    this._mediaInfo.hasVideo          = this._hasVideo;

    let duration = Math.floor(this.cms.header.duration / 1000);
    this._duration = duration;
    this._mediaInfo.duration = duration;

    
    if (this._hasAudio) {
        this._mediaInfo.audioChannelCount = this._hasAudio ? 1 : 0;
        this._mediaInfo.audioDataRate     = 64;
        this._mediaInfo.audioSampleRate   = this.cms.header.audioTrack.rate;;
        this._mediaInfo.audioCodec        = 'mp3';
    };
    if (this._hasVideo) {
        this._mediaInfo.videoDataRate     = 1000000;
        this._mediaInfo.width             = 1280;
        this._mediaInfo.height            = 720;
        if ( this.cms.header.tracks ) {
            for (var i = 0; i < this.cms.header.tracks.length; i++) {
                let track = this.cms.header.tracks[i];
                if ( track['codec'] ){
                    if ( track['codec'] == 'jpg') {
                        this._mediaInfo.width = track.width ;
                        this._mediaInfo.height= track.height ;
                        break ;
                    }
                }
            }            
        }
        let fps_num = Math.floor(1 * 1000);
        if (fps_num > 0) {
            let fps = fps_num / 1000;
            this._referenceFrameRate.fixed = true;
            this._referenceFrameRate.fps = fps;
            this._referenceFrameRate.fps_num = fps_num;
            this._referenceFrameRate.fps_den = 1000;
            this._mediaInfo.fps = fps;
        }
    };

    this._mediaInfo.hasKeyframesIndex = false;
    this._dispatch = true;  // modify by ramon: this._dispatch = false;
    this._videoInitialMetadataDispatched = true; // add by ramon
    // this._mediaInfo.metadata = onMetaData;
    //if (this._mediaInfo.isComplete()) {  // del by ramon
        this._onMediaInfo(this._mediaInfo);
    //}

    // if (Object.keys(scriptData).length > 0) {
    //     if (this._onScriptDataArrived) {
    //         this._onScriptDataArrived(Object.assign({}, scriptData));
    //     }
    // }
}


CMSIMGDemuxer.prototype._parseIMGData = function (part) {
    if (!part) {
        Log.w(this.TAG, 'Cms: Invalid jpeg packet, missing jpeg Data payload!');
        return;
    }

    if (this._hasVideoFlagOverrided === true && this._hasVideo === false) {
        // If hasVideo: false indicated explicitly in MediaDataSource,
        // Ignore all the video packets
        return;
    }

    this._parseJPEGData(part);
}

CMSIMGDemuxer.prototype._parseJPEGData = function (part) {

    let units = [], length = 0, cts = 0;

    let dts = this._timestampBase + part.header.ts;
    let keyframe = true; 
    length = part.data.byteLength; 

    let data = new Uint8Array(part.data,  0, length);
    let unit = {type: "jpeg", data: data};
    units.push(unit);
    
    if (units.length) {
        let track = this._videoTrack;
        let jpegSample = {
            units: units,
            length: length,
            isKeyframe: keyframe,
            dts: dts,
            cts: cts,
            pts: (dts + cts)
        };
        track.samples.push(jpegSample);
        track.length += length;
    }

    return ;
    
    var img = document.getElementById("imgElement"); 
    if ( ! img.src  )
    {
        var blob = new Blob( [ data ], { type: 'image/jpeg' });
        var imageUrl = (window.URL || window.webkitURL).createObjectURL(blob);
        img.setAttribute('src',imageUrl);      
       
        // var binaryString = new Array(length);
        // while (i--) {
        //     binaryString[i] = String.fromCharCode(data[i]);
        // }
        // var imgdata = 'data:image/jpg;base64,' + binaryString.join('');
        // img.setAttribute('src',imgdata);

        // var xhr = new XMLHttpRequest();
        // xhr.open('GET', '111.jpg', true);
        // xhr.responseType = 'arraybuffer';

        // xhr.onload = function (e) {
        //     var uInt8Array = new Uint8Array(xhr.response);
        //     var i = uInt8Array.length;
        //     var binaryString = new Array(i);
        //     while (i--) {
        //         binaryString[i] = String.fromCharCode(uInt8Array[i]);
        //     }
        //     var data1 = binaryString.join('');
            
        //     var imageType = xhr.getResponseHeader("Content-Type");
        //     //$('#image').attr("src", "data:" + imageType + ";base64," + data)   
    
        //     var imgdata = "data:" + imageType + ";base64," + data1;
        //     img.setAttribute('src',imgdata);
        // }
        // xhr.send();
        var video = document.getElementById("videoElement"); 
        if(video.style.display=="none"){
            //video.style.display="inline";
        }else{
            video.style.display="none";
        }

    }


}
