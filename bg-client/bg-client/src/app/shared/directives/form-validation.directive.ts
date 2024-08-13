import { Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
  selector: '[appFormValidation]'
})
export class FormValidationDirective {

  constructor(private el: ElementRef, private control: NgControl, private renderer: Renderer2) {}

  @HostListener('input') onInputChange() {
    this.validate();
  }

  @HostListener('blur') onBlur() {
    this.validate();
  }

  private validate() {
    const control = this.control.control;

    if (control) {
      if (control.invalid && (control.dirty || control.touched)) {
        this.renderer.addClass(this.el.nativeElement, 'is-invalid');
        this.renderer.removeClass(this.el.nativeElement, 'is-valid');
      } else if (control.valid && (control.dirty || control.touched)) {
        this.renderer.addClass(this.el.nativeElement, 'is-valid');
        this.renderer.removeClass(this.el.nativeElement, 'is-invalid');
      } else {
        this.renderer.removeClass(this.el.nativeElement, 'is-invalid');
        this.renderer.removeClass(this.el.nativeElement, 'is-valid');
      }
    }
  }
}
