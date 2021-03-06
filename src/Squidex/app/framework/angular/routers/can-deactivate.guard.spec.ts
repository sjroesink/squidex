/*
 * Squidex Headless CMS
 *
 * @license
 * Copyright (c) Squidex UG (haftungsbeschränkt). All rights reserved.
 */

import { Observable } from 'rxjs';

import { CanDeactivateGuard } from './can-deactivate.guard';

describe('CanDeactivateGuard', () => {
    it('should call component', () => {
        let called = false;

        const component = {
            canDeactivate: () => {
                called = true;

                return Observable.of(true);
            }
        };

        const result = new CanDeactivateGuard().canDeactivate(component);

        expect(result).toBeDefined();
        expect(called).toBeTruthy();
    });
});
