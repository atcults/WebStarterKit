define(['app'], function(app)
{
    app.controller('HomeIndexCtrl',
        [
            '$scope',
            '$http',
            'authService',
            function($scope, $http, authService)
            {
                $scope.page =
                {
                    heading: 'Welcome'
                };
            }
        ]);
});