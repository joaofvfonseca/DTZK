let router = require('express').Router();

router.get('/', function (req, res) {
    res.json({
        status: 'API Working',
        message: 'Welcome to DTZK\'s database REST API!'
    });
});

var userController = require('./userController');

router.route('/list').get(userController.list);
router.route('/add').post(userController.add);

module.exports = router;