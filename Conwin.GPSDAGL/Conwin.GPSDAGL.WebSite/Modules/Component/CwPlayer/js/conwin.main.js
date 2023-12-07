require.config({
	baseUrl: '',
	paths: {
		"lame":"/Modules/Component/Cwplayer/js/cms/lame.min",
		"alaw":"/Modules/Component/Cwplayer/js/cms/alaw",
		"cms":"/Modules/Component/Cwplayer/js/cms/cms",
		"config":"/Modules/Component/Cwplayer/js/cms/flv.js/config",
		"inherites":"/Modules/Component/Cwplayer/js/cms/flv.js/inherites",
		"event-emitter":"/Modules/Component/Cwplayer/js/cms/flv.js/utils/event-emitter",
		"exception":"/Modules/Component/Cwplayer/js/cms/flv.js/utils/exception",
		"utf8-conv":"/Modules/Component/Cwplayer/js/cms/flv.js/utils/utf8-conv",
		"browser":"/Modules/Component/Cwplayer/js/cms/flv.js/utils/browser",
		"polyfill":"/Modules/Component/Cwplayer/js/cms/flv.js/utils/polyfill",
		"logging-control":"/Modules/Component/Cwplayer/js/cms/flv.js/utils/logging-control",
		"cms-demuxer":"/Modules/Component/Cwplayer/js/cms/flv.js/demux/cms-demuxer",
		"cmsimg-demuxer":"/Modules/Component/Cwplayer/js/cms/flv.js/demux/cmsimg-demuxer",
		"flv-demuxer":"/Modules/Component/Cwplayer/js/cms/flv.js/demux/flv-demuxer",
		"exp-golomb":"/Modules/Component/Cwplayer/js/cms/flv.js/demux/exp-golomb",
		"amf-parser":"/Modules/Component/Cwplayer/js/cms/flv.js/demux/amf-parser",
		"sps-parser":"/Modules/Component/Cwplayer/js/cms/flv.js/demux/sps-parser",
		"demux-errors":"/Modules/Component/Cwplayer/js/cms/flv.js/demux/demux-errors",
		"img-remuxer":"/Modules/Component/Cwplayer/js/cms/flv.js/remux/img-remuxer",
		"mp4-remuxer":"/Modules/Component/Cwplayer/js/cms/flv.js/remux/mp4-remuxer",
		"aac-silent":"/Modules/Component/Cwplayer/js/cms/flv.js/remux/aac-silent",
		"mp4-generator":"/Modules/Component/Cwplayer/js/cms/flv.js/remux/mp4-generator",
		"features":"/Modules/Component/Cwplayer/js/cms/flv.js/core/features",
		"media-segment-info":"/Modules/Component/Cwplayer/js/cms/flv.js/core/media-segment-info",
		"mse-events":"/Modules/Component/Cwplayer/js/cms/flv.js/core/mse-events",
		"media-info":"/Modules/Component/Cwplayer/js/cms/flv.js/core/media-info",
		"mse-controller":"/Modules/Component/Cwplayer/js/cms/flv.js/core/mse-controller",
		"transmuxing-controller":"/Modules/Component/Cwplayer/js/cms/flv.js/core/transmuxing-controller",
		"transmuxing-events":"/Modules/Component/Cwplayer/js/cms/flv.js/core/transmuxing-events",
		"transmuxing-worker":"/Modules/Component/Cwplayer/js/cms/flv.js/core/transmuxing-worker",
		"transmuxer":"/Modules/Component/Cwplayer/js/cms/flv.js/core/transmuxer",
		"loader":"/Modules/Component/Cwplayer/js/cms/flv.js/io/loader",
		"io-controller":"/Modules/Component/Cwplayer/js/cms/flv.js/io/io-controller",
		"range-seek-handler":"/Modules/Component/Cwplayer/js/cms/flv.js/io/range-seek-handler",
		"fetch-stream-loader":"/Modules/Component/Cwplayer/js/cms/flv.js/io/fetch-stream-loader",
		"xhr-moz-chunked-loader":"/Modules/Component/Cwplayer/js/cms/flv.js/io/xhr-moz-chunked-loader",
		"param-seek-handler":"/Modules/Component/Cwplayer/js/cms/flv.js/io/param-seek-handler",
		"xhr-msstream-loader":"/Modules/Component/Cwplayer/js/cms/flv.js/io/xhr-msstream-loader",
		"speed-sampler":"/Modules/Component/Cwplayer/js/cms/flv.js/io/speed-sampler",
		"xhr-range-loader":"/Modules/Component/Cwplayer/js/cms/flv.js/io/xhr-range-loader",
		"flv-player":"/Modules/Component/Cwplayer/js/cms/flv.js/player/flv-player",
		"player-events":"/Modules/Component/Cwplayer/js/cms/flv.js/player/player-events",
		"native-player":"/Modules/Component/Cwplayer/js/cms/flv.js/player/native-player",
		"player-errors":"/Modules/Component/Cwplayer/js/cms/flv.js/player/player-errors",
		"logger":"/Modules/Component/Cwplayer/js/cms/flv.js/utils/logger",
        "flv": "/Modules/Component/Cwplayer/js/cms/flv.js/flv",
        "helperCallBack": "/Modules/Component/Cwplayer/js/conwin.helper",
        "Cwplayer": "/Modules/Component/Cwplayer/js/Cwplayer_simple",
        "Cwplayer1": "/Modules/Component/Cwplayer/js/Cwplayer_simple1",
    },
    shim: {
        'logging-control': {
            deps: ['event-emitter']
        },
        'fetch-stream-loader': {
            deps: ['cms']
        },
        'xhr-moz-chunked-loader': {
            deps: ['cms']
        },
        'xhr-msstream-loader': {
            deps: ['cms']
        },
        'xhr-range-loader': {
            deps: ['cms']
        },
        'flv': {
            deps: ['logging-control']
        }

    }
});