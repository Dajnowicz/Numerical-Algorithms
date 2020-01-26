#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int n = 1;

double potega(double x, int n) {

	int i;
	double wynik = 1;
	for (i = 0; i < n; i++)
	{
		wynik = (double)wynik * x;
	}

	return wynik;
}

double sumowanie_od_przodu(int koniec, double x) {

	double suma = 0;

	for (n; n <= koniec; n++) {
		suma = suma + (potega(-1, n + 1) / n) * potega(x, n);
	}

	return suma;
}

double sumowanie_od_tylu(int koniec, double x) {
	double suma = 0;

	for (n = koniec; n > 0; n--) {
		suma = suma + (potega(-1, n + 1) / n) * potega(x, n);
	}

	return suma;
}

double nastepny_wyraz_przod(int koniec, double x) {

	double suma = x;
	double q = 1;

	for (n ; n < koniec; n++) {
		q = ((potega(x, n + 1) / (n + 1)) * (n / potega(x, n)));
				if (n % 2 != 0) q = q * (-1);
			double wyraz = (potega(x, n) / n);
		suma = suma += wyraz * q;
	}

	return suma;
}


double nastepny_wyraz_tyl(int koniec, double x) {

	double suma = x;
	double q = 1;

	for (n = koniec; n > 0; n--) {
		q = ((potega(x, n + 1) / (n + 1)) * (n / potega(x, n)));
		if (n % 2 != 0) q = q * (-1);
		double wyraz = (potega(x, n) / n);
		suma = suma += wyraz * q;
	}

	return suma;
}


double mod(double number) {

	if (number < 0)
		return number * (-1);
	else return number;

}

//Wartossc b³eedu bezwzglednego niewiele mówi, gdy nic nie wiemy o ´
//rzedzie wielkosci wartosci dok³adnej.
double blad_bezwzgledny(double x, double x0) {

	return mod(x - x0);

}

double blad_wzgledny(double x, double x0) {

	return blad_bezwzgledny(x, x0) / x * 100;

}




int main() {

	int koniec = 100;
	double x = -0.999998;

	FILE *fpw = fopen("wyniki_.csv", "w");
	FILE *fpwbw = fopen("wyniki_bezw.csv", "w");
	FILE *fpww = fopen("wyniki_wzg.csv", "w");

	fprintf(fpw, "x;funkcja biblioteczna;sumowanie szeregu od przodu; sumowanie szeregu od tylu; sumowanie ciagu od przodu; sumowanie ciagu od tylu\n");
	fprintf(fpwbw, "x;funkcja biblioteczna; blad bezwzgledny od przodu;blad bezwzgledny od tylu;blad bezwzgledny ciagu p.;blad bezwzgledny ciagu t.\n");
	fprintf(fpww, "x;funkcja biblioteczna; blad wzgledny od przodu;blad wzgledny od tylu;blad wzgledny c. przod;blad wzgledny c. tyl\n");

	for (x; x < 1; x = x + 0.02) {

		fprintf(fpw, "%.6f;%.16lf;%.16lf;%.16lf;%.16lf;%.16lf;\n", x, log(1 + x), sumowanie_od_przodu(koniec, x), sumowanie_od_tylu(koniec, x), nastepny_wyraz_przod(koniec, x), nastepny_wyraz_tyl(koniec, x));
		fprintf(fpwbw, "%.6f;%.16lf;%.16lf;%.16lf;%.16lf;%.16lf;\n", x, log(1 + x), blad_bezwzgledny(log(1 + x), sumowanie_od_przodu(koniec, x)), blad_bezwzgledny(log(1 + x), sumowanie_od_tylu( koniec, x)), blad_bezwzgledny(log(1 + x), nastepny_wyraz_przod(koniec, x)), blad_bezwzgledny(log(1 + x), nastepny_wyraz_tyl(koniec, x)));
		fprintf(fpww, "%.6f;%.16lf;%.16lf;%.16lf;%.16lf;%.16lf;\n", x, log(1 + x), blad_wzgledny(log(1 + x), sumowanie_od_przodu(koniec, x)), blad_wzgledny(log(1 + x), sumowanie_od_tylu(koniec, x)), blad_wzgledny(log(1 + x), nastepny_wyraz_przod(koniec, x)), blad_wzgledny(log(1 + x), nastepny_wyraz_tyl(koniec, x)));
	}

	fclose(fpw);
	fclose(fpwbw);
	fclose(fpww);


	return 0;
}