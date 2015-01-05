define(['app'], function(app)
{
    app.controller('HomeContactCtrl',
    [
        '$scope','$http', 'ngAuthSettings',

        function($scope, $http, ngAuthSettings)
        {
            $scope.page =
            {
                heading: 'Contact Us'
            };

            $scope.test = function(){
                $http.get(ngAuthSettings.apiServiceBaseUri).then(function (results) {
                    console.log(results);
                    return results;
                });

                $http.get(ngAuthSettings.apiServiceBaseUri + "products").then(function (results) {
                    console.log(results);
                    return results;
                });
            }
        }
    ]);
});