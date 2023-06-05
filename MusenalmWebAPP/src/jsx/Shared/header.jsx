import { A } from "@solidjs/router";
 
function Header(props) {
    return (
        <header class="w-full flex flex-row px-8 py-4">
            <h1 class="grow"><A href="/">Title</A></h1>
            <div class="flex flex-row">
                <A href="/Reihen">Reihen</A>
            </div>
        </header>
    )
}

export default Header;