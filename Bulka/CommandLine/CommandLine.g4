grammar CommandLine;

input: statement* EOF;
statement: noop	| call;
noop: ';';
call: Word value*;
	
value
	: boolean
	| integer
	| real
	| string
	| int2
	| color
	;
	
boolean: Boolean; 
integer: Integer;
real: Real;
string:  DoubleQuotedString | Word;
int2: Int2 '(' integer ',' integer ')';
color: shortHexRgbColor | longHexRgbColor | shortHexRgbaColor | longHexRgbaColor | rgbColor | rgbaColor;
shortHexRgbColor: ShortHexRgbColor;
shortHexRgbaColor: ShortHexRgbaColor;
longHexRgbColor: LongHexRgbColor;
longHexRgbaColor: LongHexRgbaColor;
colorComponent: integer | real;
rgbColor: Rgb '(' colorComponent ',' colorComponent ',' colorComponent ')';
rgbaColor: (Rgba|Rgb) '(' colorComponent ',' colorComponent ',' colorComponent ',' colorComponent ')';

Int2: 'int2';
Rgb: 'rgb';
Rgba: 'rgba';
Boolean: 'true'|'false'|'yes'|'no'|'on'|'off'|'t'|'f'|'y'|'n'|'+'|'-';
Integer: '-'? [0-9]+;
Real: '-'? ([0-9]* '.' [0-9]+ | [0-9]+ '.');
DoubleQuotedString: '"' ('\\' ["\\nrt] | ~["\\\u0000-\u001F])* '"';
SingleQuotedString: '\'' ('\\' ['\\nrt] | ~['\\\u0000-\u001F])* '\'';
Word: [a-zA-Z][a-zA-Z0-9.-]* ;
ShortHexRgbColor: '#' HexDigit HexDigit HexDigit;
ShortHexRgbaColor: '#' HexDigit HexDigit HexDigit HexDigit;
LongHexRgbColor: '#' HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit;
LongHexRgbaColor: '#' HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit;

fragment HexDigit: [0-9a-fA-F];

Whitespace: [ \r\n\t]+ -> channel(HIDDEN);
