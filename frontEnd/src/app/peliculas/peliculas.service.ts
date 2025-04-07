import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LandingPageDto, PeliculaCreacionDTO, PeliculaDTO, PeliculaPostGet, PeliculaPutGet } from './pelicula';
import { formatearFecha } from '../utilidades/utilidades';

@Injectable({
  providedIn: 'root'
})
export class PeliculasService {

  constructor(private http: HttpClient) { }
  private apiUrl = environment.apiURL + 'peliculas';

  public obtenerLandingPage(): Observable<LandingPageDto> {
    return this.http.get<LandingPageDto>(this.apiUrl);
  }

  public obtenerPorId(id: number): Observable<PeliculaDTO>{
    return this.http.get<PeliculaDTO>(`${this.apiUrl}/${id}`)
  }

  public postGet(): Observable<PeliculaPostGet> {
    return this.http.get<PeliculaPostGet>(`${this.apiUrl}/postget`)
  }

  public putGet(id:number): Observable<PeliculaPutGet> {
    return this.http.get<PeliculaPutGet>(`${this.apiUrl}/PutGet/${id}`)
  }

  public filtar(valores: any): Observable<any>{
    const params = new HttpParams({fromObject: valores})
    return this.http.get<PeliculaDTO[]>(`${this.apiUrl}/filtrar`, {params, observe: 'response'});
  }

  public crear(pelicula: PeliculaCreacionDTO): Observable<number>{
    const formdata= this.construirFormData(pelicula)
    return this.http.post<number>(this.apiUrl, formdata)
  }

  public editar(id:number, pelicula: PeliculaCreacionDTO){
    const formdata=this.construirFormData(pelicula)
    formdata.forEach((value,key) => {
     console.log(`${key}:`,value)
    })

    return this.http.put(`${this.apiUrl}/${id}`, formdata)
  }

  public borrar(id: number){
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  private construirFormData(pelicula: PeliculaCreacionDTO): FormData {

    const formdata = new FormData();

    formdata.append('titulo', pelicula.titulo);
    formdata.append('resumen', pelicula.resumen);
    formdata.append('trailer', pelicula.trailer);
    formdata.append('enCines', String(pelicula.enCines));
    if(pelicula.fechaLanzamiento){
      formdata.append('fechaLanzamiento', formatearFecha(pelicula.fechaLanzamiento));
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
