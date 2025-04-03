import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PeliculaCreacionDTO, PeliculaDTO, PeliculaPostGet } from './pelicula';
import { formatearFecha } from '../utilidades/utilidades';

@Injectable({
  providedIn: 'root'
})
export class PeliculasService {

  constructor(private http: HttpClient) { }
  private apiUrl = environment.apiURL + 'peliculas';

  public obtenerPorId(id: number): Observable<PeliculaDTO>{
    return this.http.get<PeliculaDTO>(`${this.apiUrl}/${id}`)
  }

  public postGet(): Observable<PeliculaPostGet> {
    return this.http.get<PeliculaPostGet>(`${this.apiUrl}/postget`)
  }

  public crear(pelicula: PeliculaCreacionDTO){
    const formdata= this.construirFormData(pelicula)
    return this.http.post(this.apiUrl, formdata)
  }

  private construirFormData(pelicula: PeliculaCreacionDTO): FormData {

    const formdata = new FormData();

    formdata.append('titulo', pelicula.titulo);
    formdata.append('resumen', pelicula.resumen);
    formdata.append('trailer', pelicula.trailer);
    formdata.append('enCines', String(pelicula.enCines));
    if(pelicula.fechaLanzamiento){
      formdata.append('fechaLazamiento', formatearFecha(pelicula.fechaLanzamiento));
    }
    if(pelicula.poster){
      formdata.append('poster', pelicula.poster);
    }

    formdata.append('generosIds', JSON.stringify(pelicula.generosIds))
    formdata.append('cinesIds', JSON.stringify(pelicula.cinesIds))
    formdata.append('actores', JSON.stringify(pelicula.actores))

    return formdata
  }
}
