var oauthServer = require('oauth2-server'),
    oauthModel = require('../repository/OAuthRepository');

var authServer = oauthServer({
    model: oauthModel,
    grants: ['password'],
    debug: true
});

module.exports = authServer;



