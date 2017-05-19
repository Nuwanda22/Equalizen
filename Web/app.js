var express = require('express');
var path = require('path');
var fs = require('fs');
var nodeID3 = require('node-id3');
var app = express();
var bodyParser = require('body-parser');
var multiparty = require('multiparty');
var formidable = require('formidable');

app.use(bodyParser.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname + '/public')));

app.get('/', function (req, res) {
    res.sendFile(__dirname + '/public/html/index.html');
});

app.get('/player', function (req, res) {
    res.sendFile(__dirname + '/public/html/player.html');
});
app.get('/playlist', function (req, res) {
    res.sendFile(__dirname + '/public/html/playlist.html')
});

app.get('/addAudio', function (req, res) {
    res.sendFile(__dirname + '/public/html/fileuploader.html');
});

app.post('/addSongs', function (req, res) {
    var form = new multiparty.Form({});

    form.on('field', function (name, value) {
        console.log('normal field / name = ' + name + ' , value = ' + value);
    });

    form.on('part', function (part) {
        var filename;
        var size;
        if (part.filename) {
            filename = part.filename;
            size = part.byteCount;
        } else {
            part.resume();
        }

        console.log("Write Streaming file :" + filename);

        var writeStream = fs.createWriteStream(__dirname + '/tmp/' + filename);
        console.log(part);
        writeStream.filename = filename;
        part.pipe(writeStream);

        part.on('data', function (chunk) {
            console.log(filename + ' read ' + chunk.length + 'bytes');
        });

        part.on('end', function () {
            console.log(filename + ' Part read complete');

            writeStream.end();

            fs.readFile(__dirname + '/tmp/' + filename, (err, data) => {
                let songInfo = nodeID3.read(data);
                console.log(songInfo);
                fs.writeFile(__dirname + '/img/sukjinjjang/' + songInfo.title + "." + songInfo.image.mime, songInfo.image.imageBuffer, function (err) {
                    if (err) return console.log(err);
                    console.log('upload complete');
                });
            });
        });
    });

    form.on('close', function () {
        res.send("upload complete");
    });

    form.on('progress', function (byteRead, byteExpected) {
        // console.log(' Reading total  ' + byteRead + '/' + byteExpected);
    });

    form.parse(req);
});

app.listen(3000, function () {
    console.log('port 3000 is open');
});