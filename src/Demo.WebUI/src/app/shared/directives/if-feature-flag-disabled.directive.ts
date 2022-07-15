import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';
import { UserFeatureFlagService } from '@shared/services/user-feature-flag.service';

@Directive({
  selector: '[appIfFeatureFlagDisabled]'
})
export class IfFeatureFlagDisabledDirective {
  constructor(
    private readonly templateRef: TemplateRef<any>,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly userFeatureFlagService: UserFeatureFlagService
  ) {}

  @Input() set appIfFeatureFlagDisabled(featureFlag: keyof typeof FeatureFlag) {
    const isEnabled = this.userFeatureFlagService.isEnabled(featureFlag);
    if (isEnabled) {
      this.viewContainerRef.clear();
    } else {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
  }
}
