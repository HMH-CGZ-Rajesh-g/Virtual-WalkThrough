mergeInto(LibraryManager.library, {
    OpenUrl : function(url){
    window.open(Pointer_stringify(url), '_blank');
},
PlayAudio : function(url, gameobject, method){
    source = "StreamingAssets/" + Pointer_stringify(url);
    var audio = document.getElementById("AudioPlayer");
    var go = Pointer_stringify(gameobject);
    var m = Pointer_stringify(method)
    audio.src = source;
    audio.play();
    audio.onended = function(){
    unityInstance.SendMessage(go, m);
    }
},
PauseAudio : function(){
    var audio = document.getElementById("AudioPlayer");
    audio.pause();
},
ResumeAudio : function(){
    var audio = document.getElementById("AudioPlayer");
    audio.play();
}
});