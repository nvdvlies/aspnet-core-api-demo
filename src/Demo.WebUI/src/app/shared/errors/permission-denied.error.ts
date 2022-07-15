export class PermissionDeniedError extends Error {
  constructor(message?: string) {
    super(message ?? 'Permission denied.');
    this.name = 'PermissionDeniedError';
    this.stack = new Error().stack;
  }
}
