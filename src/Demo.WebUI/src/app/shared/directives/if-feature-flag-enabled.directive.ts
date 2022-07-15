import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';
import { UserFeatureFlagService } from '@shared/services/user-feature-flag.service';

@Directive({
  selector: '[appIfFeatureFlagEnabled]'
})
export class IfFeatureFlagEnabledDirective {
  constructor(
    private readonly templateRef: TemplateRef<any>,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly userFeatureFlagService: UserFeatureFlagService
  ) {}

  @Input() set appIfFeatureFlagEnabled(featureFlag: keyof typeof FeatureFlag) {
    const isEnabled = this.userFeatureFlagService.isEnabled(featureFlag);
    if (isEnabled) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }
}
