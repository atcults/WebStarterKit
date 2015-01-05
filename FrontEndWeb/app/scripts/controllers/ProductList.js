define(['app', 'services/productService'], function(app)
{
    app.controller('ProductCtrl',
        [
            '$scope','$http', 'productService', 'ngAuthSettings',

            function($scope, $http, productService, ngAuthSettings)
            {
                $scope.page =
                {
                    heading: 'Product List'
                };

                $scope.brandList = [
                    {brandName : "Nokia", brandId : 1},
                    {brandName : "Apple", brandId : 2},
                    {brandName : "HTC", brandId : 3},
                    {brandName : "LG", brandId : 4},
                    {brandName : "Asus", brandId : 5}
                ];

                /*for(var i = 0 ; i < $scope.brandList.length ; i++){
                    for(var j = 0 ; j < $scope.productList.length ; j++){
                        if($scope.brandList[i].brandId == $scope.productList[j].brandId){
                            var k =  $scope.brList.some(function (item) {
                                return (item.brandId == $scope.brandList[i].brandId);
                            });
                            if(!k){$scope.brList.push($scope.brandList[i]);}
                        }
                    }
                }*/

                productService.getProducts().then(function (response) {
                        console.log(response);
                        $scope.productList = response;
                    },
                    function (err) {
                        console.dir(err);
                        //$scope.message = err.error_description;
                    });
            }
        ]);
});