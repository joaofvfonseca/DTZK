User = require('./userModel');

exports.list = function (req, res) {
    User.find({
        pub: {
            $exists: true
        }
    }, 'pub', function (err, listResults) {
        if (err) {
            res.json({
                status: 'error',
                message: err,
            });
        } else {
            res.json({
                status: 'success',
                message: 'Listing all players in leaderboard',
                data: listResults,
            })
        };
    });
};

exports.add = function (req, res) {
    var newUser = new User();
    newUser.gen_id = req.body.gen_id;
    newUser.pub.username = req.body.username;
    newUser.pub.score = req.body.score;


    User.find({
        gen_id: newUser.gen_id
    }, 'gen_id', function (err, results) {
        if (results == 0) {
            newUser.save(function (err) {
                if (err) {
                    res.json({
                        status: "error",
                        message: err,
                    });
                } else {
                    res.json({
                        status: 'success',
                        message: 'Added user to leaderboard',
                        data: newUser.pub,
                    })
                };
            });
        } else {
            results[0].pub = newUser.pub;
            results[0].last_mod = newUser.last_mod;
            results[0].save(function (err) {
                if (err) {
                    res.json(err);
                } else {
                    res.json({
                        status: 'success',
                        message: 'Updated user ' + results[0].pub.username,
                        data: results[0].pub,
                    })
                };
            });
        }
    });
};