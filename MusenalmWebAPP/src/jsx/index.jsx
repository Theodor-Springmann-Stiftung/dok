import { render } from "solid-js/web";
import state from "./globalstate"
import { Router, Route, Routes } from "@solidjs/router";

import Header from "./Shared/header";
import Footer from "./Shared/footer";
import Home from "./Pages/home";
import Reihen from "./Pages/reihen";

function App() {

  return (
    <>
    <div class="">
        <Header></Header>
        <div class="mx-auto max-w-7xl">
        <Routes>
          <Route path="/" component={Home} />
          <Route path="/Reihen" component={Reihen} />
        </Routes>
        </div>
      </div>
      <Footer></Footer>
    </>
  );
}

render(() => (
  <Router>
    <App />
  </Router>
), document.getElementById("root"));
