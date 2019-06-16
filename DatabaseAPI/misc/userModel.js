var mongoose = require('mongoose');

var userSchema = mongoose.Schema({
    gen_id: {
        type: String,
        required: true,
    },
    last_mod: {
        type: Date,
        default: Date.now,
    },
    pub: {
        username: {
            type: String,
            required: true,
        },
        score: {
            type: Number,
            required: true,
        },
    },
});

var User = module.exports = mongoose.model('user', userSchema);

module.exports.get = function (callback, limit) {
    User.find(callback).limit(limit);
};