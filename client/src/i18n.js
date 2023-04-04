import i18n from "i18next";
import { initReactI18next } from "react-i18next";

i18n
    .use(initReactI18next)
    .init({
        resources: {
            en: {
                translation: {
                    //English translations here
                    "Sign In": "Sign In",
                    "Forgot password?": "Forgot password?",
                    "Remember me": "Remember me",
                    "Don't have an account? Sign Up": "Don't have an account? Sign Up",
                    "Email Address": "Email Address",
                    "Password": "Password",
                },
            },
            no: {
                translation: {
                    "Sign in": "Logg inn",
                    "Forgot password?": "Glemt passord?",
                    "Remember me": "Husk meg",
                    "Don't have an account? Sign Up": "Har du ikke konto? Registrer deg",
                    "Email Address": "E-postadresse",
                    "Password": "Passord",
                },
            },
        },
        lng: "en",
        fallbackLng: "en",
    });

export default i18n;