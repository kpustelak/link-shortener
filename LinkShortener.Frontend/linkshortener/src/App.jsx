import './App.css'
import '../components/FormComponent.jsx';
import FormComponent from "../components/FormComponent.jsx";
import SideComponent from "../components/SvgComponent.jsx";

function App() {

  return (
    <div className="2xl:bg-[url(../public/meow.svg)] bg-no-repeat bg-right bg-contain duration-0 transition-none">
        <div className="flex bg-[url(../public/splashundertext.svg)] bg-no-repeat 2xl:bg-left bg-center">
            <FormComponent />
            <SideComponent className="flex"/>
        </div>
        <div className="flex pb-2 pl-4">
            <a href="https://github.com/kpustelak">
                <img src="../public/gh.svg"  alt="Github icon" />
            </a>
            <a href="https://www.linkedin.com/in/kornel-pustelak-274205284/?skipRedirect=true">
                <img src="../public/le.svg" className="px-3" alt="Linkedin Icon"/>
            </a>
            <a className="font-roboto-flex font-bold text-3xl text-center" href="https://pustelak.com">pustelak.com</a>
        </div>
    </div>
  )
}

export default App
