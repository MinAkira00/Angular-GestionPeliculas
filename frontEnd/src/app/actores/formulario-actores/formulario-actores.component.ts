import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { actorCreacionDTO, actorDTO } from '../actor';

@Component({
  selector: 'app-formulario-actores',
  templateUrl: './formulario-actores.component.html',
  styleUrls: ['./formulario-actores.component.css']
})
export class FormularioActoresComponent implements OnInit {
  constructor(private formBuilder: FormBuilder) {}

  form: FormGroup;

  @Input()
  modelo: actorDTO;

  @Input()
  errores:string[] = [];


  @Output()
  OnSubmit: EventEmitter<actorCreacionDTO> = new EventEmitter<actorCreacionDTO>();

  imagenCambiada = false;
  
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      nombre: [
        '',
        {
          validators: [Validators.required],
        },
      ],
      fechaNacimiento: '',
      foto: '',
      biografia: ''
    });

    if (this.modelo !== undefined){
      this.form.patchValue(this.modelo)
    }

  }


  archivoSeleccionado(file: File){
    this.imagenCambiada = true;
    this.form.get('foto').setValue(file);
  }

  cambioMarkdown(texto: string){
    this.form.get('biografia').setValue(texto);
  }

  onSubmit(){
    const formValue = this.form.value;
    if(!this.imagenCambiada && this.modelo && this.modelo.foto){
      formValue.foto = this.modelo.foto;
    }else if (!this.imagenCambiada){
      formValue.foto = null;
    }
    this.OnSubmit.emit(formValue);
  }

}
