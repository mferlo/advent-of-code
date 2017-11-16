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
    console.log(`${re.source} matched ${s} at ${m.index} (${replacement}): ${result}`);
    results.push(result);
  }
  if (results.length) {
    console.log("Results: " + JSON.stringify(results, null, 2));
  }
  return results;
};

const str = x => JSON.stringify(x, null, 2);

const applyAllReplacements = input => {
  const results = new Set();
  for (const d of data) {
    const re = new RegExp(d.target, "g");
    replaceEach(input, re, d.replacement).forEach(s => results.add(s));
    console.log(str(results));
  }
  console.log(str(results));
  return results;
};

const part1 = () => console.log(applyAllReplacements(molecule).size);

const part2 = () => {
  let input = [ "e" ];
  let i = 0;
  while (true) {
    i++;
    console.log("INPUT: " + str(input));
    const a = applyAllReplacements(input);
    console.log(str(a));
    break;
  }
}

// part1();
part2();
