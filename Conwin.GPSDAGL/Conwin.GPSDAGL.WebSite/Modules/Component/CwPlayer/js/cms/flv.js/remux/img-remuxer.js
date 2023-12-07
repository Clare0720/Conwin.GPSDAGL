
//  img play
function IMGRemuxer(config) {
    this.TAG = 'IMGRemuxer';

    this._config = config;
    this._isLive = (config.isLive === true) ? true : false;

    this._dtsBase = -1;
    this._dtsBaseInited = false;
    this._audioDtsBase = Infinity;
    this._videoDtsBase = Infinity;
    this._audioNextDts = undefined;
    this._videoNextDts = undefined;
    this._audioStashedLastSample = null;
    this._videoStashedLastSample = null;

    this._audioMeta = null;
    this._videoMeta = null;

    this._audioSegmentInfoList = new MediaSegmentInfoList('audio');
    this._videoSegmentInfoList = new MediaSegmentInfoList('video');

    this._onInitSegment = null;
    this._onMediaSegment = null;

    // While only FireFox supports 'audio/mp4, codecs="mp3"', use 'audio/mpeg' for chrome, safari, ...
    this._mp3UseMpegAudio = !Browser.firefox;

    this._fillAudioTimestampGap = this._config.fixAudioTimestampGap;

    this._videoTrack = {type: 'jpeg', id: 1, sequenceNumber: 0, samples: [], length: 0};

    this._playTimer = null;
    this._playInterval = 960;

    this._mediaElement = null;
    this._imgElement = null; 
    
    this._loadEnd = false;
    
}

IMGRemuxer.prototype.destroy = function () {
    this._disablePlayTimer();
    this._dtsBase = -1;
    this._dtsBaseInited = false;
    this._audioMeta = null;
    this._videoMeta = null;
    this._audioSegmentInfoList.clear();
    this._audioSegmentInfoList = null;
    this._videoSegmentInfoList.clear();
    this._videoSegmentInfoList = null;
    this._onInitSegment = null;
    this._onMediaSegment = null;
}

IMGRemuxer.prototype.attachMediaElement = function(mediaElement) {
    if ( "VIDEO" == mediaElement.tagName ){
        this._mediaElement = mediaElement;
    } else if ( "IMG" == mediaElement.tagName ) {
        this._imgElement = mediaElement;
    }
}

IMGRemuxer.prototype.bindDataSource = function (producer) {
    producer.onDataAvailable(this.remux.bind(this));
    producer.onTrackMetadata(this._onTrackMetadataReceived.bind(this));
    this._loadEnd = false;
    return this;
}

IMGRemuxer.prototype.onInitSegment = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onInitSegment;
    } else {
        this._onInitSegment = callback;
    }
}

/* prototype: function onMediaSegment(type: string, mediaSegment: MediaSegment): void
   MediaSegment: {
   type: string,
   data: ArrayBuffer,
   sampleCount: int32
   info: MediaSegmentInfo
   }
*/
IMGRemuxer.prototype.onMediaSegment = function (callback) {
    if (typeof callback === 'undefined') {
        return this._onMediaSegment;
    } else {
        this._onMediaSegment = callback;
    }
}

IMGRemuxer.prototype.insertDiscontinuity = function () {
    this._audioNextDts = this._videoNextDts = undefined;
}

IMGRemuxer.prototype.seek = function (originalDts) {
    this._audioStashedLastSample = null;
    this._videoStashedLastSample = null;
    this._videoSegmentInfoList.clear();
    this._audioSegmentInfoList.clear();
}

IMGRemuxer.prototype.remux = function (audioTrack, videoTrack) {
    if (!this._onMediaSegment) {
        throw new IllegalStateException('MP4Remuxer: onMediaSegment callback must be specificed!');
    }
    if (!this._dtsBaseInited) {
        this._calculateDtsBase(audioTrack, videoTrack);
    }
    this._remuxVideo(videoTrack);
    //this._remuxAudio(audioTrack);
}

IMGRemuxer.prototype._onTrackMetadataReceived = function (type, metadata) {

}

IMGRemuxer.prototype._calculateDtsBase = function (audioTrack, videoTrack) {
    if (this._dtsBaseInited) {
        return;
    }

    if (audioTrack.samples && audioTrack.samples.length) {
        this._audioDtsBase = audioTrack.samples[0].dts;
    }
    if (videoTrack.samples && videoTrack.samples.length) {
        this._videoDtsBase = videoTrack.samples[0].dts;
    }

    this._dtsBase = Math.min(this._audioDtsBase, this._videoDtsBase);
    this._dtsBaseInited = true;
}

IMGRemuxer.prototype._calculatePlayInterval = function () {

    if ( this._videoTrack.samples.length > 3 ) {
        var sample1 = this._videoTrack.samples[0];
        var sample2 = this._videoTrack.samples[1];
        var sample3 = this._videoTrack.samples[2];
        let interval1 = sample2.pts - sample1.pts ;
        let interval2 = sample3.pts - sample2.pts ;
        if ( ( interval1 > 0 ) && ( interval2 > 0 ) ) {
            interval1 = parseInt( ( interval1 + interval2 ) / 2 );
            if ( Math.abs(this._playInterval - interval1) > 100 ){
                this._playInterval = interval1;
                this._disablePlayTimer();
                this._enablePlayTimer(this._playInterval);
            }
        }
    } else {
        return ;
    }

}

IMGRemuxer.prototype.flushStashedSamples = function () {
    this._loadEnd = true;
}

IMGRemuxer.prototype._remuxAudio = function (audioTrack, force) {

}

IMGRemuxer.prototype._playImg = function () {
    //debugger;

    if ( ( this._imgElement  ) && ( this._videoTrack.samples.length > 0) ) {
        var sample = this._videoTrack.samples.shift();
        if ( sample.units.length > 0 ) {
            var uint = sample.units.shift();
            var data = uint.data ;
            
            var blob = new Blob( [ data ], { type: 'image/jpeg' });
            var imageUrl = (window.URL || window.webkitURL).createObjectURL(blob);
            this._imgElement.setAttribute('src',imageUrl); 

            sample.units = [];

            Log.v(this.TAG, '播放一帧图片');
        }
 
        if ( this._mediaElement ) {
            if(this._mediaElement.style.display=="none"){
                //this._mediaElement.style.display="inline";
            }else{
                this._mediaElement.style.display="none";
            }
        }

        this._calculatePlayInterval();

    } else if ( this._loadEnd ) {
        this._disablePlayTimer();
    }

}

IMGRemuxer.prototype._remuxVideo = function (videoTrack, force) {

    let track = videoTrack;
    let samples = track.samples;

    if (!samples || samples.length === 0) {
        return;
    }

    for (let i = 0; i < samples.length; i++) {
        let sample = samples[i];
        this._videoTrack.samples.push(sample);
        Log.v(this.TAG, '加入一帧图片');
    }

    track.samples = [];
    track.length = 0;

    if ( ! this._playTimer ){
        this._playImg();
        this._enablePlayTimer(this._playInterval);
    }

}

IMGRemuxer.prototype._disablePlayTimer = function () {
    if (this._playTimer) {
        self.clearInterval(this._playTimer);
        this._playTimer = null;
    }
}

IMGRemuxer.prototype._enablePlayTimer = function (timeInterval) {
    if (this._playTimer == null) {
        this._playTimer = self.setInterval(
            this._playImg.bind(this),
            timeInterval);
    }
}

