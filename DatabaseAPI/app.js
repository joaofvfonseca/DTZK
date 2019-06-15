let express = require('express');
let bodyParser = require('body-parser');
let mongoose = require('mongoose')
let api_routes = require('./misc/routes');

let app = express();
let port = process.env.PORT || 8080;

mongoose.connect('REDACTED', {useNewUrlParser: true});
var db = mongoose.connection;

app.use(bodyParser.json());
app.use('/dtzk/api', api_routes);

app.listen(port, function () {
    console.log('Running REST API on port ' + port)
});