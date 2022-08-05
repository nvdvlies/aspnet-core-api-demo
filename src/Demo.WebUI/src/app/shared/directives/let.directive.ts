import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appLet]'
})
export class LetDirective {
  @Input()
  set appLet(context: unknown) {
    this.context.$implicit = this.context.ngVar = context;

    if (!this.hasView) {
      this.vcRef.createEmbeddedView(this.templateRef, this.context);
      this.hasView = true;
    }
  }

  private hasView: boolean = false;

  private context: {
    $implicit: unknown;
    ngVar: unknown;
  } = {
    $implicit: null,
    ngVar: null
  };

  constructor(private templateRef: TemplateRef<any>, private vcRef: ViewContainerRef) {}
}
