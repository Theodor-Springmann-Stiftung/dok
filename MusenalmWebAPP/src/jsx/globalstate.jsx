import { createSignal, createMemo, createRoot, createResource } from "solid-js";
import createDictionary from './helpers';

const fTVOC = async () => {
  const res = await fetch('http://localhost:5163/Vocabulary/TVOC');
  return res.json();
}

const fFVOC = async () => {
  const res = await fetch('http://localhost:5163/Vocabulary/FVOC');
  return res.json();
}

const fOrte = async() => {
  const res = await fetch('http://localhost:5163/Orte');
  return res.json();
}

const fAkteure = async() => {
  const res = await fetch('http://localhost:5163/Akteure');
  return res.json();
}

function createState() {
  const [TVOC] = createResource(fTVOC);
  const [FVOC] = createResource(fFVOC);
  const [Orte] = createResource(fOrte);
  const [Akteure] = createResource(fAkteure);

  const TVOC_D = createMemo(() => createDictionary(TVOC()));
  const FVOC_D = createMemo(() => createDictionary(FVOC()));
  const Orte_D = createMemo(() => createDictionary(Orte()));
  const Akteure_D = createMemo(() => createDictionary(Akteure()));

  const [title, setTitle] = createSignal("Musenalm KorrDB");

  return { title, setTitle, TVOC_D, FVOC_D, Orte_D, Akteure_D };
}


export default createRoot(createState);