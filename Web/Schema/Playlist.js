var mongoose = require('mongoose');
var playlistSchema = new mongoose.Schema({
    playlistName : String,
    numOfSong : Number,
    username : String,
});

module.exports = mongoose.model('Playlist',playlistSchema);