package ApiServer;

import java.io.IOException;
import javax.servlet.Filter;
import javax.servlet.FilterChain;
import javax.servlet.FilterConfig;
import javax.servlet.ServletException;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.stereotype.Component;

@Component
public class CORSFilter implements Filter {

    public void init(FilterConfig arg) throws ServletException {
        System.out.println("Init CORS Filter");
    }

    public void doFilter(ServletRequest req, ServletResponse resp, FilterChain chain) throws IOException, ServletException {

        HttpServletRequest request = (HttpServletRequest) req;
        HttpServletResponse response=(HttpServletResponse) resp;

        long startTime = System.currentTimeMillis();
        System.out.println(request.getMethod() + "::" + request.getRequestURL().toString() + ":: Start Time=" + System.currentTimeMillis());
        System.out.println("Authorization: " + request.getHeader("Authorization"));

        response.setHeader("Access-Control-Allow-Origin", "*");
        response.setHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, TRACE, OPTIONS");
        response.setHeader("Access-Control-Allow-Headers", "Content-Type, Authorization, Content-Length, X-Requested-With");
        response.setHeader("Access-Control-Max-Age", "3600");

        if(request.getMethod().equals("OPTIONS"))
        {
            System.out.println("Bypass options method");
            response.flushBuffer();
            return;
        }
        
        chain.doFilter(req, resp);

        System.out.println("Request=" + request.getRequestURL().toString() + "::Status=" + response.getStatus() + ":: End Time=" + System.currentTimeMillis() + ":: Time Taken=" + (System.currentTimeMillis() - startTime));
        System.out.println("\n\n");
    }

    public void destroy() {
        System.out.println("Destroying CORS Filter");
    }

}
