/*
 To Generate Token :
 localhost:3000/oauth/token
 POST
 PAYLOAD
 username=admin&password=admin&client_id=thom&client_secret=store&grant_type=password
 */

var express = require('express');
var router = express.Router();
var oAuthServer = require('../service/authService');

router.all('/token', oAuthServer.grant(), function(req, res){
    console.log('token');
    
});

router.get('/', oAuthServer.authorise(), function(req, res) {
    res.render('index', { title: 'Auth Module', nodeVar : 'Node' });
});


module.exports = router;