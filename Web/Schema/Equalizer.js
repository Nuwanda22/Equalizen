var mongoose = require('mongoose');
var equalizerSchema = new mongoose.Schema({
    _60hz : Number,
    _170hz : Number,
    _350hz : Number,
    _1000hz : Number,
    _3500hz : Number,
    _10000hz : Number
});

module.exports = mongoose.model('Equalizer',equalizerSchema);