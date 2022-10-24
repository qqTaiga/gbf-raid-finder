import { ComponentMeta, ComponentStory } from '@storybook/react';

import { RoundButton } from './index';

export default {
    title: 'Components/Round Button',
    component: RoundButton,
    argTypes: {
        onClick: { action: 'clicked' },
    },
} as ComponentMeta<typeof RoundButton>;

const Template: ComponentStory<typeof RoundButton> = (args) => <RoundButton {...args} />;

export const Primary = Template.bind({});
Primary.args = {
    label: '+',
};
