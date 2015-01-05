var db = require('./tingoProvider');

var provider = {};

var getCollection = function(callback) {
    db.collection('users', function(error, collection) {
        if( error ) callback(error);
        else callback(null, collection);
    });
};

//find all users
provider.findAll = function(callback) {
    getCollection(function(error, collection) {
        if( error ) callback(error);
        else {
            collection.find().toArray(function(error, results) {
                if( error ) callback(error);
                else callback(null, results);
            });
        }
    });
};

//find an user by ID
provider.findById = function(id, callback) {
    getCollection(function(error, collection) {
        if( error ) callback(error);
        else {
            collection.findOne({_id: id}, function(error, result) {
                if( error ) callback(error);
                else callback(null, result);
            });
        }
    });
};

//find an user by AccessToken
provider.findByAccessToken = function(token, callback) {
    getCollection(function(error, collection) {
        if( error ) callback(error);
        else {
            collection.findOne({accessToken: token}, function(error, result) {
                if( error ) callback(error);
                else callback(null, result);
            });
        }
    });
};

//save new user
provider.save = function(users, callback) {
    getCollection(function(error, collection) {
        if( error ) callback(error);
        else {
            if( typeof(users.length)=="undefined")
                users = [users];

            for( var i =0;i< users.length;i++ ) {
                user = users[i];
                user.created_at = new Date();
            }

            collection.insert(users, function() {
                callback(null, users);
            });
        }
    });
};

// update an user
provider.update = function(id, users, callback) {
    getCollection(function(error, collection) {
        if( error ) callback(error);
        else {
            collection.update(
                {_id: id},
                users,
                function(error, users) {
                    if(error) callback(error);
                    else callback(null, users)
                });
        }
    });
};

//delete user
provider.delete = function(id, callback) {
    getCollection(function(error, collection) {
        if(error) callback(error);
        else {
            collection.remove(
                {_id: id },
                function(error, user){
                    if(error) callback(error);
                    else callback(null, user)
                });
        }
    });
};


module.exports = provider;