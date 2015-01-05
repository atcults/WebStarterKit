/**
 * Created by Sunny on 11/18/2014.
 */
define(function () {

    if (typeof String.prototype.startsWith != 'function') {
        // see below for better implementation!
        String.prototype.startsWith = function (str){
            return this.indexOf(str) == 0;
        };
    }

    return {};
});

