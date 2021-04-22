'use strict';

const molecule = "CRnCaCaCaSiRnBPTiMgArSiRnSiRnMgArSiRnCaFArTiTiBSiThFYCaFArCaCaSiThCaPBSiThSiThCaCaPTiRnPBSiThRnFArArCaCaSiThCaSiThSiRnMgArCaPTiBPRnFArSiThCaSiRnFArBCaSiRnCaPRnFArPMgYCaFArCaPTiTiTiBPBSiThCaPTiBPBSiRnFArBPBSiRnCaFArBPRnSiRnFArRnSiRnBFArCaFArCaCaCaSiThSiThCaCaPBPTiTiRnFArCaPTiBSiAlArPBCaCaCaCaCaSiRnMgArCaSiThFArThCaSiThCaSiRnCaFYCaSiRnFYFArFArCaSiRnFYFArCaSiRnBPMgArSiThPRnFArCaSiRnFArTiRnSiRnFYFArCaSiRnBFArCaSiRnTiMgArSiThCaSiThCaFArPRnFArSiRnFArTiTiTiTiBCaCaSiRnCaCaFYFArSiThCaPTiBPTiBCaSiThSiRnMgArCaF";

const data = [
  { target: "Al", replacement: "ThF" },
  { target: "Al", replacement: "ThRnFAr" },
  { target: "B", replacement: "BCa" },
  { target: "B", replacement: "TiB" },
  { target: "B", replacement: "TiRnFAr" },
  { target: "Ca", replacement: "CaCa" },
  { target: "Ca", replacement: "PB" },
  { target: "Ca", replacement: "PRnFAr" },
  { target: "Ca", replacement: "SiRnFYFAr" },
  { target: "Ca", replacement: "SiRnMgAr" },
  { target: "Ca", replacement: "SiTh" },
  { target: "F", replacement: "CaF" },
  { target: "F", replacement: "PMg" },
  { target: "F", replacement: "SiAl" },
  { target: "H", replacement: "CRnAlAr" },
  { target: "H", replacement: "CRnFYFYFAr" },
  { target: "H", replacement: "CRnFYMgAr" },
  { target: "H", replacement: "CRnMgYFAr" },
  { target: "H", replacement: "HCa" },
  { target: "H", replacement: "NRnFYFAr" },
  { target: "H", replacement: "NRnMgAr" },
  { target: "H", replacement: "NTh" },
  { target: "H", replacement: "OB" },
  { target: "H", replacement: "ORnFAr" },
  { target: "Mg", replacement: "BF" },
  { target: "Mg", replacement: "TiMg" },
  { target: "N", replacement: "CRnFAr" },
  { target: "N", replacement: "HSi" },
  { target: "O", replacement: "CRnFYFAr" },
  { target: "O", replacement: "CRnMgAr" },
  { target: "O", replacement: "HP" },
  { target: "O", replacement: "NRnFAr" },
  { target: "O", replacement: "OTi" },
  { target: "P", replacement: "CaP" },
  { target: "P", replacement: "PTi" },
  { target: "P", replacement: "SiRnFAr" },
  { target: "Si", replacement: "CaSi" },
  { target: "Th", replacement: "ThCa" },
  { target: "Ti", replacement: "BP" },
  { target: "Ti", replacement: "TiTi" },
  { target: "e", replacement: "HF" },
  { target: "e", replacement: "NAl" },
  { target: "e", replacement: "OMg" },
];

const replace = (s, i, len, replacement) =>
  s.slice(0, i) + replacement + s.slice(i + len);

const replaceEach = (s, re, replacement) => {
  const results = [];
  let m;
  while (m = re.exec(s)) {
    const result = replace(s, m.index, re.source.length, replacement);
    results.push(result);
  }
  return results;
};

const str = x => JSON.stringify(x, null, 2);

const applyAllReplacements = input => {
  const results = new Set();
  for (const d of data) {
    const re = new RegExp(d.target, "g");
    const replacements = replaceEach(input, re, d.replacement);
    replacements.forEach(r => results.add(r));
  }
  return Array.from(results);
};

const part1 = () => console.log(applyAllReplacements(molecule).size);

// ... yeah OK, this ain't gonna work. Need to come up with an actual algorithm here.
const part2 = () => {
  let input = [ "e" ];
  let i = 0;
  while (true) {
    i++;
    const replacements = input.map(applyAllReplacements).reduce((arr, x) => arr.concat(x), []);
    const distinct = new Set(replacements);
    console.log(i + " " + distinct.size);
    if (distinct.has(molecule)) {
      console.log(i);
      break;
    }
    input = Array.from(distinct).filter(p => p.length <= molecule.length);
  }
}


//part1();
part2();
