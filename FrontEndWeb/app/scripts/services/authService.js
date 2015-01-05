define(['app'], function(app)
{
    
    //NODE SERVER
    app.constant('ngAuthSettings', {
        apiServiceBaseUri: 'http://localhost:8001/',
        clientId: 'thom',
        clientSecret: 'store'
    });

    //JAVA SERVER
    /*app.constant('ngAuthSettings', {
        apiServiceBaseUri: 'http://localhost:90/JavaServer/',
        clientId: 'client1',
        clientSecret: 'client1'
    });*/

    //DOT NET SERVER
    /*app.constant('ngAuthSettings', {
        apiServiceBaseUri: 'http://localhost:2210/',
        clientId: 'client1',
        clientSecret: 'client1'
    });*/


    app.service('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {

        var self = this;

        self.serviceBase = ngAuthSettings.apiServiceBaseUri;

        self.authentication = {
            isAuth: false,
            userName: "",
            useRefreshTokens: false
        };

        self.saveRegistration = function (registration) {

            self.logOut();

            return $http.post(self.serviceBase + 'api/account/register', registration).then(function (response) {
                return response;
            });

        };

        self.login = function (loginData) {

            var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password + "&client_id=" + ngAuthSettings.clientId + "&client_secret=" + ngAuthSettings.clientSecret;

            if (loginData.useRefreshTokens) {
                console.log('refresh token required');
            }

            console.log(data);

            var deferred = $q.defer();

             $http.post(self.serviceBase + 'oauth/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                 if (loginData.useRefreshTokens) {
                    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: response.refresh_token, useRefreshTokens: true });
                 }
                 else {
                    localStorageService.set('authorizationData', { token: response.access_token, userName: loginData.userName, refreshToken: "", useRefreshTokens: false });
                 }

                 self.authentication.isAuth = true;
                 self.authentication.userName = loginData.userName;
                 self.authentication.useRefreshTokens = loginData.useRefreshTokens;

                 deferred.resolve(response); }).error(function (err, status) {
                    self.logOut();
                    deferred.reject(err);
             });

            return deferred.promise;

        };

        self.logOut = function () {

            localStorageService.remove('authorizationData');

            self.authentication.isAuth = false;
            self.authentication.userName = "";
            self.authentication.useRefreshTokens = false;

        };

        self.fillAuthData = function () {
            var authData = localStorageService.get('authorizationData');
            if (authData) {
                self.authentication.isAuth = true;
                self.authentication.userName = authData.userName;
                self.authentication.useRefreshTokens = authData.useRefreshTokens;
            }
        };

        self.refreshToken = function ()
        {
            var deferred = $q.defer();

            var authData = localStorageService.get('authorizationData');

            if (authData) {

                if (authData.useRefreshTokens) {

                    var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId + "&client_secret=" + ngAuthSettings.clientSecret;

                     $http.post(self.serviceBase + 'auth/token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

                     localStorageService.set('authorizationData', { token: response.access_token, userName: response.userName, refreshToken: response.refresh_token, useRefreshTokens: true });

                     deferred.resolve(response);

                     }).error(function (err, status) {
                         self.logOut();
                         deferred.reject(err);
                     });
                }
            }

            return deferred.promise;
        };
    }]);

    app.service('httpRequestInterceptor', ['$q', '$injector','$location', 'localStorageService', function ($q, $injector,$location, localStorageService) {

        var self = this;

        self.request = function (config) {
            config.headers = config.headers || {};

            var authData = localStorageService.get('authorizationData');

            if (authData) {
                config.headers.Authorization = 'Bearer ' + authData.token;
            }
            console.log(config);
            return config;
        };

        self.responseError = function (rejection) {
            console.dir(rejection);
            if (rejection.status === 0 || rejection.status === 401 || rejection.status === 400) {
                var authService = $injector.get('authService');
                var authData = localStorageService.get('authorizationData');

                if (authData) {
                    if (authData.useRefreshTokens) {
                        $location.path('/refresh');
                        return $q.reject(rejection);
                    }
                }
                authService.logOut();
                $location.path('/account/login');
            }
            return $q.reject(rejection);
        };
    }]);

});