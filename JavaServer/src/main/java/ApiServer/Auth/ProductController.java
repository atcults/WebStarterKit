package ApiServer.Auth;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.servlet.http.HttpServletRequest;


@Controller
@RequestMapping(value = "/api/products")
public class ProductController {

    @RequestMapping(method = RequestMethod.GET)
    public
    @ResponseBody
    ResponseEntity<Product[]> GetAll(HttpServletRequest request) {
        
        Product[] products = new Product[4];
        
        products[0] = new Product(1, "One");
        products[1] = new Product(2, "Two");
        products[2] = new Product(3, "Three");
        products[3] = new Product(4, "Four");
        
        return new ResponseEntity<>(products, HttpStatus.OK);
    }
}

