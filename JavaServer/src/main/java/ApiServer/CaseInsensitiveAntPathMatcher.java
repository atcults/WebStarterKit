package ApiServer;

import org.springframework.util.AntPathMatcher;

public class CaseInsensitiveAntPathMatcher extends AntPathMatcher {

    @Override
    public boolean match(String pattern, String string) {
        System.out.println(pattern + ":" + string + ":" + super.match(pattern.toLowerCase(), string.toLowerCase()));
        return super.match(pattern.toLowerCase(), string.toLowerCase()); // make this according to your need
    }
}
