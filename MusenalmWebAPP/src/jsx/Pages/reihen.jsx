import { createResource, createSignal, Show, For } from "solid-js"
import state from "../globalstate"

export default function Reihen(props) {
    const getData = async () => {
        const res = await fetch('http://localhost:5163/Reihen');
        return res.json();
    }
    const [d] = createResource(getData);
    return (
        <>
        <table class="px-2">
            <thead>
                <tr>
                    <th class="px-1">Name</th>
                    <th class="px-1">Anz. Bände</th>
                </tr>
            </thead>
            <tbody>
                <Show when={d()} fallback={<p>Daten werden geladen</p>}>
                    <For each={d()}>{(reihe) => 
                        <Reihe reihe={reihe}></Reihe>
                    }</For>
                </Show>
            </tbody>
        </table>
        </>
    )
}

function Reihe(props) {
    const getData = async (throwaway) => {
        const res = await Promise.all(
            props.reihe.reL_Baende_Reihen.map(
                rel => fetch("http://localhost:5163/Baende/" + rel.bandID)
                            .then((b) => b.json())
            ))
        return res;
    } 

    const [showDetails, setShowDetails] = createSignal(false);
    const [d] = createResource(showDetails, getData)
    const toggleShowDetails = (e) => {
        setShowDetails(!showDetails());
        e.preventDefault();
    }
    return (
        <>
        <tr class="cursor-pointer hover:bg-slate-200 odd:bg-slate-50 even:bg-slate-100" onClick={(e) => toggleShowDetails(e)}>
            <Show when={showDetails()} fallback={(
                <>
                <td class="px-1.5 py-0.5">
                    {props.reihe.sortiername}
                </td>
                <td class="px-1.5 py-0.5 text-right">
                    {props.reihe.reL_Baende_Reihen.length}
                </td>
                </>
            )}>
                <td colspan="2" class="px-1.5 py-0.5 pb-2 bg-indigo-100 border-indigo-200 border-2">
                    {props.reihe.sortiername}
                    <div>
                    <Show when={d()} fallback={<p>Daten werden geladen...</p>}>
                    <div class="grid grid-cols-[5%_1fr_5%] gap-x-2 px-2 py-1 mb-0.5">
                        <div>
                            ID
                        </div>
                        <div>
                            Titel
                        </div>
                        <div class="text-right">
                            Jahr
                        </div>
                    </div>
                        <For each={d()}>{(band) =>
                            <div class="grid grid-cols-[5%_1fr_5%] gap-y-1 gap-x-2 px-2 py-1 border border-indigo-200 bg-indigo-50 mb-1">
                            <div>
                                {band.id}
                            </div>
                            <div>
                                {band.titelTranskription}
                            </div>
                            <div class="text-right">
                                {band.jahr}
                            </div>
                            </div>
                        }</For>
                    </Show>
                    </div>
                </td>
            </Show>  
        </tr>
        </>
    )
}