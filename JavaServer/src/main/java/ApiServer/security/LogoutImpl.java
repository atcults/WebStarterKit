package ApiServer.security;

import org.springframework.security.core.Authentication;
import org.springframework.security.oauth2.common.DefaultOAuth2AccessToken;
import org.springframework.security.oauth2.provider.token.InMemoryTokenStore;
import org.springframework.security.web.authentication.logout.LogoutSuccessHandler;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;


public class LogoutImpl implements LogoutSuccessHandler {


    private InMemoryTokenStore tokenStore;


    public InMemoryTokenStore getTokenStore() {
        return tokenStore;
    }


    public void setTokenStore(InMemoryTokenStore tokenStore) {
        this.tokenStore = tokenStore;
    }


    @Override
    public void onLogoutSuccess(HttpServletRequest paramHttpServletRequest,
                                HttpServletResponse paramHttpServletResponse,
                                Authentication paramAuthentication) throws IOException, ServletException {
        RemoveAccess(paramHttpServletRequest);
        paramHttpServletResponse.getOutputStream().write("\n\tYou Have Logged Out successfully.".getBytes());

    }


    public void RemoveAccess(HttpServletRequest req) {

        String tokens = req.getHeader("Authorization");
        System.out.println(tokens);
        String value = tokens.substring(tokens.indexOf(" ")).trim();
        DefaultOAuth2AccessToken token = new DefaultOAuth2AccessToken(value);
        System.out.println(token);
        tokenStore.removeAccessToken(value);
        System.out.println("\n\tAccess Token Removed Successfully!!!!!!!!");

    }

}
