var express = require('express');
var path = require('path');
var fs = require('fs');
var nodeID3 = require('node-id3');
var app = express();
var bodyParser = require('body-parser');
var multiparty = require('multiparty');
var ejs = require('ejs');
var cookieParser = require('cookie-parser');
var session = require('express-session');
var mongoose = require('mongoose');
var User = require('./Schema/User');
var Song = require('./Schema/Song');
var Playlist = require('./Schema/Playlist');


app.use(bodyParser.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname + '/public')));
app.use(cookieParser());
app.use(session({
    secret: 'Equalizen',
    resave: false,
    saveUninitialized: true
}));

app.use('/player', function (req, res, next) {
    if (req.session.username == undefined) {
        res.redirect('/signIn');
    }
    next();
});

app.set('views', path.join(__dirname, 'views'))
app.set('view engine', 'ejs')


app.get('/', function (req, res) {
    res.sendFile(__dirname + '/public/html/index.html');
});

app.get('/player', function (req, res) {
    res.sendFile(__dirname + '/public/html/player.html');
});
app.get('/playlist', function (req, res) {
    var songs;
    Song.find({ username: req.session.username }, (err, result) => {
        songs = result;
        res.render("playlist", { songs: result });
    })
    // res.sendFile(__dirname + '/public/html/playlist.html')
});

app.get('/addAudio', function (req, res) {
    res.sendFile(__dirname + '/public/html/fileuploader.html');
});

app.get('/signIn', function (req, res) {
    res.sendFile(__dirname + '/public/html/sign.html');
});

app.get('/signout', function (req, res) {
    req.session.username = undefined;
    res.redirect('/');
})

app.get('/getSong/:songName', function (req, res) {
    var songFileURL = req.params.songName;
    Song.findOne({ songURL: songFileURL }, (err, result) => {
        if (err) console.log(err);
    });
    req.session.nowPlay = req.params.songName;
    res.send('/songs/' + req.session.username + '/' + req.params.songName);
});

app.get('/resetDB', function (req, res) {
    Song.remove({}, (err) => {
        if (err) console.log(err);
        User.remove({}, (err) => {
            if (err) console.log(err);
            res.redirect('/');
        });
    })
})

app.post('/signUp', function (req, res) {
    User.find({ id: req.body.id }, function (err, results) {
        if (err) console.log(err);
        if (results.length === 0) {
            var user = new User({
                id: req.body.id,
                password: req.body.pwd,
                name: req.body.name
            });
            user.save(function (err) {
                if (err) console.log(err);
                res.send("<script>location.href = '/signIn'</script>");
            });
        } else {
            res.send(`invalidation data ${results}`);
        }
    });
});

app.post('/signIn', function (req, res) {
    User.find({ id: req.body.id, password: req.body.pwd }, function (err, results) {
        if (err) console.log(err);
        if (results.length === 1) {
            req.session.username = results[0].name;
            console.log(req.session.username);
            console.log(req.session);
            res.send(`
            <script>location.href = '/player'</script>`
            );
        } else if (results.length > 1) {
            res.send('database has multiple user data');
        } else if (results.length === 0) {
            res.send('please enter correct id/password');
        }
    });
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
        if (!fs.existsSync(__dirname + '/public/songs/' + req.session.username)) {
            fs.mkdir(__dirname + '/public/songs/' + req.session.username);
        }

        var writeStream = fs.createWriteStream(__dirname + '/public/' + '/songs/' + req.session.username + '/' + filename);
        writeStream.filename = filename;
        part.pipe(writeStream);

        part.on('data', function (chunk) {
            console.log(filename + ' read ' + chunk.length + 'bytes');
        });

        part.on('end', function () {
            console.log(filename + ' Part read complete');

            writeStream.end();

            console.log(`${__dirname}/public/songs/${req.session.username}/${filename}`);
            fs.readFile(__dirname + '/public/songs/' + req.session.username + '/' + filename, (err, data) => {
                let songInfo = nodeID3.read(data);
                if (!fs.existsSync(__dirname + '/public/img/' + req.session.username)) {
                    fs.mkdir(__dirname + '/public/img/' + req.session.username)
                }
                fs.writeFile(__dirname + '/public/img/' + req.session.username + '/' + songInfo.title + "." + songInfo.image.mime, songInfo.image.imageBuffer, function (err) {
                    if (err) return console.log(err);
                    console.log('upload complete');
                });
                var song = new Song();
                song.title = songInfo.title;
                song.artist = songInfo.artist;
                song.album = songInfo.album;
                song.username = req.session.username;
                song.equalizer = [0, 0, 0, 0, 0, 0]
                song.albumCoverURL = `${songInfo.title}.${songInfo.image.mime}`;
                song.songURL = filename;
                song.username = req.session.username;

                song.save(function () {
                    Song.find({}, (err, results) => {
                        console.log(results);
                    });
                });
            });
        });
    });

    form.on('close', function () {
        res.send(`
        upload complete
        Hello world
        <script>
        window.addEventListener("message", (event) => {
            if (event.data == "Reset")
                location.href = "/html/audioUploader.html"
        });
        </script>
        `);
    });

    form.on('progress', function (byteRead, byteExpected) {
        // console.log(' Reading total  ' + byteRead + '/' + byteExpected);
    });

    form.parse(req);
});

app.post('/changeEqualizer', (req, res) => {
    var nbs = [60, 170, 350, 1000, 3500, 10000];
    var result = eval(`(${Object.keys(req.body)[0]})`);
    console.log(result.hz);
    var nowEqualizer;
    Song.findOne({ username: req.session.username, songURL: req.session.nowPlay }, (err, song) => {
        console.log(song.equalizer);
        nowEqualizer = song.equalizer;
        console.log(nowEqualizer);
        nowEqualizer[result.hz] = result.val;
    });
    Song.update({
        username: req.session.username,
        songURL: req.session.nowPlay
    }, {
            equalizer: nowEqualizer
        }, (err, results) => {
            if (err) console.log(err);
            console.log(results);
        })
});

function connect() {
    mongoose.Promise = global.Promise;
    mongoose.connect('localhost:27017', function (err) {
        if (err) console.log(err);
        console.log('mongodb connected');
    });

    mongoose.connection.on('error', function (err) {
        console.log(err);
    });

    mongoose.connection.on('open', function () {
        console.log('success open connection');
    });

    mongoose.connection.on('disconnect', connect);
    require('./Schema/User')
    require('./Schema/Song');
    require('./Schema/Playlist');
    require('./Schema/Equalizer');
}

app.listen(3000, function () {
    console.log('port 3000 is open');
    connect();
});