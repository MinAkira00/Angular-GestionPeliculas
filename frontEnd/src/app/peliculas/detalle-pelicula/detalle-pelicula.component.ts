import { Component, OnInit, Sanitizer } from '@angular/core';
import { PeliculasService } from '../peliculas.service';
import { ActivatedRoute } from '@angular/router';
import { PeliculaDTO } from '../pelicula';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { RatingService } from 'src/app/rating/rating.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-detalle-pelicula',
  templateUrl: './detalle-pelicula.component.html',
  styleUrls: ['./detalle-pelicula.component.css']
})
export class DetallePeliculaComponent implements OnInit {

  constructor(private peliculasService: PeliculasService, private activatedRoute: ActivatedRoute, private sanitazer: DomSanitizer, private ratingService: RatingService) { }

  pelicula: PeliculaDTO;
  fechaLanzamiento: Date;
  trailerURL: SafeResourceUrl;

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.peliculasService.obtenerPorId(params.id).subscribe(pelicula => {
        this.pelicula = pelicula;
        this.fechaLanzamiento = new Date(this.pelicula.fechaLanzamiento);
        this.trailerURL = this.generarURLYoutubeEmbed(this.pelicula.trailer)
      })
    })
  }

  rated(puntuacion: number){
    this.ratingService.rate(this.pelicula.id, puntuacion)
      .subscribe(() => {
        Swal.fire("Exitoso", "Su voto ha sido recibido", 'success')
      })
  }


  generarURLYoutubeEmbed(url: any):SafeResourceUrl{
    if(!url){return '';}

    var video_id = url.split('v=')[1];
    var posicionesAmpersand = video_id.indexOf('&');
    if(posicionesAmpersand !== -1){
      video_id = video_id.substring(0, posicionesAmpersand);
    }
    return this.sanitazer
      .bypassSecurityTrustResourceUrl(`http://www.youtube.com/embed/${video_id}`)
  }

}
