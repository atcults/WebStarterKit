package ApiServer.Auth;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.oauth2.common.OAuth2AccessToken;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;

import javax.servlet.http.HttpServletRequest;

@Controller
@RequestMapping(value = "/api/users")
public class UsersController {

    @RequestMapping(method = RequestMethod.GET)
    public
    @ResponseBody
    ResponseEntity<String> GetAllUsers(HttpServletRequest request) {
        String response = "Protected Resource(GetAllUsers) Accessed !!!! Returning from My Resource GetAllUsers\n";
        System.out.println(request.getParameter(OAuth2AccessToken.ACCESS_TOKEN));
        return new ResponseEntity<>(response, HttpStatus.OK);
    }
}
