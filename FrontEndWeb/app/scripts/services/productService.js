define(['app'], function(app)
{
    app.service('productService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {

        var self = this;

        self.serviceBase = ngAuthSettings.apiServiceBaseUri + 'api/';

        self.getProducts = function () {

            var deferred = $q.defer();

            console.log(self.serviceBase);
            
             $http.get(self.serviceBase + 'products').success(function (response) {
                 console.log(response);
                 deferred.resolve(response); }).error(function (err, status) {
                    deferred.reject(err);
             });

            return deferred.promise;

        };

    }]);
});