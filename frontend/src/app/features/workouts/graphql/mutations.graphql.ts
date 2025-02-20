import gql from "graphql-tag";

export const CREATE_WORKOUT = gql`
  mutation CreateWorkout(
    $title: String!,
    $description: String!,
    $durationInMinutes: Int!,
    $level: DifficultyLevel!,
    $imageUrl: Upload,
    $exercises: [CreateExerciseDtoInput!]!
  ) {
    createWorkout(
      input: {
        title: $title
        description: $description
        durationInMinutes: $durationInMinutes
        level: $level
        imageUrl: $imageUrl
        exercises: $exercises
      }
    ) {
      id
      title
      description
      durationInMinutes
      level
      imageUrl
    }
  }
`;
