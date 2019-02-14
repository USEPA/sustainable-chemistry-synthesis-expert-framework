/*  This grammar is based upon the OpenSMILES specification grammar grammar
    obtained from http://opensmiles.org/spec/open-smiles-2-grammar.html
 */

grammar smiles;

/*
 * Parser Rules
 */

//smiles   : atom ( chain | branch )* EOF;
//chain    : ( bond? ( atom | ringbond ) )+;
//branch   : '(' bond? smiles+ ')';

smiles          : (chain? EOF) | /* explicitly handle case where the entire smile is enclosed in parenthesis*/ ('(' chain ')')? EOF;
chain           :  branched_atom | chain branched_atom | chain bond branched_atom | chain dot branched_atom;
branched_atom   : atom (bond? ringbond)* branch*;
branch          : '(' chain ')' | '(' bond chain ')' | '(' dot chain ')' |  /* handle redundant parenthesis */ '(' branch ')';
ringbond        : ('%' DIGIT DIGIT) | DIGIT;

bond            : '-' | '=' | '#' | '$' | ':' | '\\' | '/';
dot             : '.';

atom            : ('[' isotope? symbol chiral? hcount? charge? atomclass? ']') | organic | aromatic;
// Consider bracket atom as an inline option for atoms as this prevents problems with the bracket atom for organics.
// bracket_atom    :  ('[' isotope? symbol chiral? hcount? charge? atomclass? ']');
symbol          : element /*| organic | aromatic */| WILDCARD;
organic         : 'B' | 'C' | 'N' | 'O' | 'S' | 'P' | 'F' | 'Cl' | 'Br' | 'I';
aromatic        : 'b' | 'c' | 'n'| 'o' | 'p' | 's'| 'se' | 'as';

// Calling out Halogens Separately...
halogen         : 'X';


chiral          :  '@'
            |  '@@'
            |  '@TH1' | '@TH2'
            |  '@AL1' | '@AL2'
            |  '@SP1' | '@SP2' | '@SP3'
            |  ('@TB1' DIGIT?) | ('@TB2' DIGIT?) | '@TB3'| '@TB30'
            |  ('@OH1' DIGIT?) | ('@OH2' DIGIT?) | '@OH3'| '@OH30';

charge          : '-'
            |  ('-' DIGIT? DIGIT)
            |  '+'
            |  ('+' DIGIT? DIGIT)
            |  '--'           /*deprecated*/
            |  '++';           /*deprecated*/

hcount          : 'H' |  ('H' DIGIT);

atomclass       : ':' DIGIT+;

isotope         : DIGIT DIGIT? DIGIT?;

/*
 * Lexer Rules
 */
element         : 'H' | 'He' | 'Li' | 'Be' | 'B' | 'C' | 'N' | 'O' | 'F' | 'Ne' 
            | 'Na' | 'Mg' | 'Al' | 'Si' | 'P' | 'S' | 'Cl' | 'Ar' | 'K' 
            | 'Ca' | 'Sc' | 'Ti' | 'V' | 'Cr' | 'Mn' | 'Fe' | 'Co' | 'Ni' 
            | 'Cu' | 'Zn' | 'Ga' | 'Ge' | 'As' | 'Se' | 'Br' | 'Kr' | 'Rb' 
            | 'Sr' | 'Y' | 'Zr' | 'Nb' | 'Mo' | 'Tc' | 'Ru' | 'Rh' | 'Pd' 
            | 'Ag' | 'Cd' | 'In' | 'Sn' | 'Sb' | 'Te' | 'I' | 'Xe' | 'Cs' 
            | 'Ba' | 'Hf' | 'Ta' | 'W' | 'Re' | 'Os' | 'Ir' | 'Pt' | 'Au' 
            | 'Hg' | 'Tl' | 'Pb' | 'Bi' | 'Po' | 'At' | 'Rn' | 'Fr' | 'Ra' 
            | 'Rf' | 'Db' | 'Sg' | 'Bh' | 'Hs' | 'Mt' | 'Ds' | 'Rg' | 'Cn' 
            | 'Fl' | 'Lv' | 'La' | 'Ce' | 'Pr' | 'Nd' | 'Pm' | 'Sm' | 'Eu' 
            | 'Gd' | 'Tb' | 'Dy' | 'Ho' | 'Er' | 'Tm' | 'Yb' | 'Lu' | 'Ac' 
            | 'Th' | 'Pa' | 'U' | 'Np' | 'Pu' | 'Am' | 'Cm' | 'Bk' | 'Cf' 
            | 'Es' | 'Fm' | 'Md' | 'No' | 'Lr';
ORGANIC_SYMBOL         : 'B' | 'C' | 'N' | 'O' | 'S' | 'P' | 'F' | 'Cl' | 'Br' | 'I';
AROMATIC_SYMBOL        : 'b' | 'c' | 'n'| 'o' | 'p' | 's'| 'se' | 'as';
WILDCARD        : '*';
DIGIT           : [0-9];
