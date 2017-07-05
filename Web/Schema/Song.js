var mongoose = require('mongoose');
var songSchema = new mongoose.Schema({
    title : String,
    artist : String,
    album : String,
    username : String,
    albumCoverURL : String,
    songURL : String,
    equalizer : [{type: Number, default:0}]
});

module.exports = mongoose.model('Song',songSchema);