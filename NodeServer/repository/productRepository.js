    var provider = {};

    // In-memory dataStores:
    var products = [
            {
                id : '1',
                name: 'HTC Desire',
                description: 'Product description 1',
                imageUrl : '1',
                offerPrice : 2000,
                actualPrice : 4000,
                discount : '50%',
                brandId : 3
            },
            {
                id : '2',
                name: 'LG G2',
                description: 'Product description 2',
                imageUrl : '2',
                offerPrice : 1500,
                actualPrice : 4500,
                discount : '67%',
                brandId : 4
            },
            {
                id : '3',
                name: 'Asus Zenfone 5',
                description: 'Product description 3',
                imageUrl : '3',
                offerPrice : 22000,
                actualPrice : 30000,
                discount : '26%',
                brandId : 5
            },
            {
                id : '4',
                name: 'iPhone 5s',
                description: 'Product description 4',
                imageUrl : '4',
                offerPrice : 3500,
                actualPrice : 4000,
                discount : '12.5%',
                brandId : 2
            }
        ];

    // Debug function to dump the state of the data stores
    provider.dump = function() {
        console.log('products', products);
    };

    //find all products
    provider.findAll = function(callback) {
        callback(null, products);
    };

    //find an product ID
    provider.findById = function(id, callback) {
        for(var i = 0, len = products.length; i < len; i++) {
            var ent = products[i];
            if(ent.id === id) {
                return callback(null, ent);
            }
        }
        callback(null, null);
    };

    //save new product
    provider.save = function(product, callback) {
        products.push(product);
        callback(null, product);
    };

    //delete product by product ID
    provider.delete = function(id, callback) {
        for(var i = 0 ; i < products.length ; i++){
            var pObj = products[i];
            if(pObj.id === id){
                products.splice(i);
                break;
            }
        }
        callback(null, id);
    };

    //update product by product ID
    provider.update = function(id, product, callback){
        for(var i = 0 ; i < products.length ; i++){
            if(products[i].id === id){
                products[i].name = product.name;
                products[i].description = product.description;
                break;
            }
        }
        callback(null, product);
    };

    module.exports = provider;