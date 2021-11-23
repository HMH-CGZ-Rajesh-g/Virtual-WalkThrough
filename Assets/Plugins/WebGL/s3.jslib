mergeInto(LibraryManager.library, {
    PreSignLink : function(accessKeyId,secretAccessKey,sessionToken,region,bucket,key, gameObject, method){
        AWS.config.update({accessKeyId:Pointer_stringify(accessKeyId),secretAccessKey: Pointer_stringify(secretAccessKey),sessionToken : Pointer_stringify(sessionToken), region:  Pointer_stringify(region)})
        var s3 = new AWS.S3();
        this.arrayofElements = [];
        this.arrayUUID = [];
        var go = Pointer_stringify(gameObject);
        var m = Pointer_stringify(method);
        var presignedURL = s3.getSignedUrl('getObject', {
        Bucket: Pointer_stringify(bucket),
        Key: Pointer_stringify(key),
        Expires: 3600
        })
    console.log("Complete URL : " + presignedURL)
    unityInstance.SendMessage(go, m, presignedURL);
    },
    PreSignPut : function(aKeyId,sKey,region,bucket,feedbackLink, gameObject, method){
        var s3 = new AWS.S3({accessKeyId : Pointer_stringify(aKeyId), secretAccessKey: Pointer_stringify(sKey), region : "us-east-2",signatureVersion: 'v4'});
        var go = Pointer_stringify(gameObject);
        var m = Pointer_stringify(method);
        var dt = new Date().getTime();
        var uuid = 'xx-5xx'.replace(/[xy]/g, function (c) {
        var r = (dt + Math.random() * 16) % 16 | 0;
        dt = Math.floor(dt / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        var params = { Bucket: Pointer_stringify(bucket) , Key: 'Feedback.json', Expires : 3600};
        var url = s3.getSignedUrl('putObject', params);
            console.log(go);

        unityInstance.SendMessage(go, m, url);
    },
   
    CreatePostLink : function(accessKeyId,secretAccessKey,sessionToken,region,bucket, feedbackLink, id, count){
        AWS.config.update({accessKeyId:Pointer_stringify(accessKeyId),secretAccessKey: Pointer_stringify(secretAccessKey),sessionToken : Pointer_stringify(sessionToken), region:  Pointer_stringify(region)})
        var s3 = new AWS.S3();
        var datas = [];
        for(i=0;i<count;i++){
        var dt = new Date().getTime();
        var uuid = 'xx-5xx'.replace(/[xy]/g, function (c) {
        var r = (dt + Math.random() * 16) % 16 | 0;
        dt = Math.floor(dt / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        arrayUUID.push(uuid +'.json');
        console.log("uuid :: " + uuid);
        var params = {
        Bucket: Pointer_stringify(bucket),
        Fields: {
          key: uuid +'.json'
        }
      }
        s3.createPresignedPost(params, function(err, data){
        if (err) {
          console.error('Presigning post data encountered an error', err);
        } else {
          datas.push(data);
        }
      })
        }
        for(i=0;i<datas.length;i++){
          window.open(Pointer_stringify(feedbackLink)+"?id="+Pointer_stringify(id)+"&data=" + JSON.stringify(datas[i]), '_blank');
          window.focus();
        }
    },
    GetAWSBucketElements : function(accessKeyId,secretAccessKey,sessionToken,region,bucket, gameObject, method) {
    AWS.config.update({accessKeyId:Pointer_stringify(accessKeyId),secretAccessKey: Pointer_stringify(secretAccessKey),sessionToken : Pointer_stringify(sessionToken), region:  Pointer_stringify(region)})
    var s3 = new AWS.S3({ signatureVersion: 'v2' });
    var params = {
        Bucket: Pointer_stringify(bucket)
    };
    var go = Pointer_stringify(gameObject);
    var m = Pointer_stringify(method);
    var arrayofObjects = [];
    s3.listObjects(params, function(err, data){
        if(err){
          console.log(err.message);
          unityInstance.SendMessage(go, m, "");
        }
        else {
          for (i=0;i<data.Contents.length;i++){
          var element = data.Contents[i];
          if (arrayofElements.indexOf(element.Key) < 0){
                        arrayofElements.push(element.Key);
                        arrayofObjects.push(element.Key);
                        console.log("added :: " + element.Key)
                    }
                    else{
                        console.log("exists");
                    }
            }
        }
            console.log("Array of Elements :: " + arrayofElements);
            console.log("Array of Objects :: " + arrayofObjects);
            if(arrayofObjects.length > 0){
            console.log("Sending :: " + JSON.stringify(arrayofObjects));
            unityInstance.SendMessage(go, m, JSON.stringify(arrayofObjects));
            }
            else{
              unityInstance.SendMessage(go, m, "");
            }
    })
},
 GetAWSBucketObjects : function(accessKeyId,secretAccessKey,sessionToken,region,bucket,key, gameObject, method) {
    AWS.config.update({accessKeyId:Pointer_stringify(accessKeyId),secretAccessKey: Pointer_stringify(secretAccessKey),sessionToken : Pointer_stringify(sessionToken), region:  Pointer_stringify(region)})
    var s3 = new AWS.S3({ signatureVersion: 'v2' });
    var go = Pointer_stringify(gameObject);
    var m = Pointer_stringify(method);
    var params = {
        Bucket: Pointer_stringify(bucket),
        Key: Pointer_stringify(key)
    };
    s3.getObject(params, function(err, data){
        if (err) console.log(err.message);
        else {
            var objectData = data.Body.toString('utf-8');
            console.log(JSON.parse(objectData));
            unityInstance.SendMessage(go, m, objectData);
        }
    })
}
});
