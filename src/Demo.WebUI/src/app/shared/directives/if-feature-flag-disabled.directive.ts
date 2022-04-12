import { Directive, ElementRef, Input } from '@angular/core';
import { FeatureFlag } from '@shared/enums/feature-flag.enum';
import { FeatureFlagService } from '@shared/services/feature-flag.service';

@Directive({
  selector: '[appIfFeatureFlagDisabled]'
})
export class IfFeatureFlagDisabledDirective {
  constructor(private elementRef: ElementRef, private featureFlagService: FeatureFlagService) {}

  @Input() set appIfFeatureFlagDisabled(featureFlag: keyof typeof FeatureFlag) {
    const isEnabled = this.featureFlagService.isEnabled(featureFlag);
    if (isEnabled) {
      this.elementRef.nativeElement.remove();
    }
  }
}
