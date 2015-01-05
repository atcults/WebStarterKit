var express = require('express');
var oAuthServer = require('../service/authService');

var router = express.Router();

router.get('/', oAuthServer.authorise(), function (req, res) {
    console.log('product');
    var repo = require("../repository/productRepository");
    repo.findAll(function(err, result){
        res.send(result);
    });
});

module.exports = router;
