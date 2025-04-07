import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PeliculaCreacionDTO, PeliculaDTO } from '../pelicula';
import { MultipleSelectorModel } from 'src/app/utilidades/selector-mutilple/MultipleSelectorModel';
import { actorPeliculaDTO } from 'src/app/actores/actor';


@Component({
  selector: 'app-formulario-pelicula',
  templateUrl: './formulario-pelicula.component.html',
  styleUrls: ['./formulario-pelicula.component.css']
})
export class FormularioPeliculaComponent implements OnInit {
  constructor(private formBuilder: FormBuilder) {}

  form: FormGroup;

  @Input()
  errores: string[] = []

  @Input()
  modelo: PeliculaDTO;

  @Output()
  OnSubmit: EventEmitter<PeliculaCreacionDTO> = new EventEmitter<
    PeliculaCreacionDTO
  >();

  @Input()
  generosNoSeleccionados: MultipleSelectorModel[] = [];
  @Input()
  generosSeleccionados: MultipleSelectorModel[] = [];

  @Input()
  cinesNoSeleccionados: MultipleSelectorModel[] = [];
  @Input()
  cinesSeleccionados: MultipleSelectorModel[] = [];

  @Input()
  actoresSeleccionados: actorPeliculaDTO[] = [];

  imagenCambiada = false;

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      titulo: [
        '',
        {
          validators: [Validators.required],
        },
      ],
      resumen: '',
      enCines: false,
      trailer: '',
      fechaLanzamiento: '',
      poster: '',
      generosIds: '',
      cinesIds: '',
      actores: ''
    });

    if (this.modelo !== undefined) {
      this.form.patchValue(this.modelo);
    }
  }

  archivoSeleccionado(archivo: File) {
    this.imagenCambiada = true;
    this.form.get('poster').setValue(archivo);
  }

  changeMarkdown(texto) {
    this.form.get('resumen').setValue(texto);
  }

  guardarCambios() {
    const generosIds = this.generosSeleccionados.map(val => val.llave);
    this.form.get('generosIds').setValue(generosIds)

    const cinesIds = this.cinesSeleccionados.map(val => val.llave);
    this.form.get('cinesIds').setValue(cinesIds)

    const actores = this.actoresSeleccionados.map(val => {
      return {id: val.id, personaje: val.personaje}
    })
    this.form.get('actores').setValue(actores);

    const formValue = this.form.value;
    if(!this.imagenCambiada && this.modelo && this.modelo.poster){
      formValue.poster = this.modelo.poster;
    }else if (!this.imagenCambiada){
      formValue.poster = this.modelo.poster || null;;
    }
    this.OnSubmit.emit(formValue);
  }

}
